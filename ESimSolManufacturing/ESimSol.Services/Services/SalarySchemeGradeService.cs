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
    public class SalarySchemeGradeService : MarshalByRefObject, ISalarySchemeGradeService
    {
        #region Private functions and declaration
        private SalarySchemeGrade MapObject(NullHandler oReader)
        {
            SalarySchemeGrade oSalarySchemeGrade = new SalarySchemeGrade();
            oSalarySchemeGrade.SSGradeID = oReader.GetInt32("SSGradeID");
            oSalarySchemeGrade.Name = oReader.GetString("Name");
            oSalarySchemeGrade.ParentID = oReader.GetInt32("ParentID");
            oSalarySchemeGrade.Note = oReader.GetString("Note");
            oSalarySchemeGrade.MinAmount = oReader.GetDouble("MinAmount");
            oSalarySchemeGrade.MaxAmount = oReader.GetDouble("MaxAmount");
            oSalarySchemeGrade.IsActive = oReader.GetBoolean("IsActive");
            oSalarySchemeGrade.IsLastLayer = oReader.GetBoolean("IsLastLayer");
            oSalarySchemeGrade.SalarySchemeID = oReader.GetInt32("SalarySchemeID");
            return oSalarySchemeGrade;
        }

        private SalarySchemeGrade CreateObject(NullHandler oReader)
        {
            SalarySchemeGrade oSalarySchemeGrade = MapObject(oReader);
            return oSalarySchemeGrade;
        }

        private List<SalarySchemeGrade> CreateObjects(IDataReader oReader)
        {
            List<SalarySchemeGrade> oSalarySchemeGrade = new List<SalarySchemeGrade>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SalarySchemeGrade oItem = CreateObject(oHandler);
                oSalarySchemeGrade.Add(oItem);
            }
            return oSalarySchemeGrade;
        }

        #endregion

        #region Interface implementation
        public SalarySchemeGradeService() { }

        public SalarySchemeGrade IUD(SalarySchemeGrade oSalarySchemeGrade, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update || nDBOperation == (int)EnumDBOperation.Approval)
                {
                    reader = SalarySchemeGradeDA.IUD(tc, oSalarySchemeGrade, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oSalarySchemeGrade = new SalarySchemeGrade();
                        oSalarySchemeGrade = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = SalarySchemeGradeDA.IUD(tc, oSalarySchemeGrade, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oSalarySchemeGrade.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oSalarySchemeGrade = new SalarySchemeGrade();
                oSalarySchemeGrade.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oSalarySchemeGrade;
        }


        public SalarySchemeGrade Get(int nSalarySchemeGradeID, Int64 nUserId)
        {
            SalarySchemeGrade oSalarySchemeGrade = new SalarySchemeGrade();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SalarySchemeGradeDA.Get(tc, nSalarySchemeGradeID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalarySchemeGrade = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oSalarySchemeGrade = new SalarySchemeGrade();
                oSalarySchemeGrade.ErrorMessage = ex.Message;
                #endregion
            }

            return oSalarySchemeGrade;
        }

        public List<SalarySchemeGrade> Gets(string sSQL, Int64 nUserID)
        {
            List<SalarySchemeGrade> oSalarySchemeGrades = new List<SalarySchemeGrade>();
            SalarySchemeGrade oSalarySchemeGrade = new SalarySchemeGrade();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalarySchemeGradeDA.Gets(tc, sSQL);
                oSalarySchemeGrades = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oSalarySchemeGrade.ErrorMessage = ex.Message;
                oSalarySchemeGrades.Add(oSalarySchemeGrade);
                #endregion
            }

            return oSalarySchemeGrades;
        }

        #endregion
    }
}
