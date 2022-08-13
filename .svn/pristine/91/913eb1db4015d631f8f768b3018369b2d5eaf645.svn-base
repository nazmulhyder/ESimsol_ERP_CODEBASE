using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
    public class FabricQCGradeService : MarshalByRefObject, IFabricQCGradeService
    {
        #region Private functions and declaration
        private FabricQCGrade MapObject(NullHandler oReader)
        {
            FabricQCGrade oFabricQCGrade = new FabricQCGrade();
            oFabricQCGrade.FabricQCGradeID = oReader.GetInt32("FabricQCGradeID");
            oFabricQCGrade.Name = oReader.GetString("Name");
            oFabricQCGrade.SLNo = oReader.GetInt32("SLNo");
            oFabricQCGrade.QCGradeType = (EnumFBQCGrade)oReader.GetInt32("QCGradeType");
            oFabricQCGrade.LastUpdateBy = oReader.GetInt32("LastUpdateBy");
            oFabricQCGrade.LastUpdateDateTime = Convert.ToDateTime(oReader.GetDateTime("LastUpdateDateTime"));
            oFabricQCGrade.LastUpdateByName = oReader.GetString("LastUpdateByName");
            oFabricQCGrade.MinValue = oReader.GetDouble("MinValue");
            oFabricQCGrade.MaxValue = oReader.GetDouble("MaxValue");
            oFabricQCGrade.GradeSL = (EnumExcellColumn)oReader.GetInt32("GradeSL");

            return oFabricQCGrade;
        }
        private FabricQCGrade CreateObject(NullHandler oReader)
        {
            FabricQCGrade oFabricQCGrade = new FabricQCGrade();
            oFabricQCGrade = MapObject(oReader);
            return oFabricQCGrade;
        }

        private List<FabricQCGrade> CreateObjects(IDataReader oReader)
        {
            List<FabricQCGrade> oFabricQCGrade = new List<FabricQCGrade>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricQCGrade oItem = CreateObject(oHandler);
                oFabricQCGrade.Add(oItem);
            }
            return oFabricQCGrade;
        }
        #endregion
        #region Interface implementation
        public FabricQCGrade Save(FabricQCGrade oFabricQCGrade, Int64 nUserID)
        {
            string sRefChildIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricQCGrade.FabricQCGradeID <= 0)
                {

                    reader = FabricQCGradeDA.InsertUpdate(tc, oFabricQCGrade, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricQCGradeDA.InsertUpdate(tc, oFabricQCGrade, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricQCGrade = new FabricQCGrade();
                    oFabricQCGrade = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oFabricQCGrade = new FabricQCGrade();
                    oFabricQCGrade.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFabricQCGrade;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricQCGrade oFabricQCGrade = new FabricQCGrade();
                oFabricQCGrade.FabricQCGradeID = id;
                DBTableReferenceDA.HasReference(tc, "FabricQCGrade", id);
                FabricQCGradeDA.Delete(tc, oFabricQCGrade, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public FabricQCGrade Get(int id, Int64 nUserId)
        {
            FabricQCGrade oFabricQCGrade = new FabricQCGrade();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricQCGradeDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricQCGrade = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricQCGrade", e);
                #endregion
            }
            return oFabricQCGrade;
        }
        public List<FabricQCGrade> Gets(Int64 nUserID)
        {
            List<FabricQCGrade> oFabricQCGrades = new List<FabricQCGrade>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricQCGradeDA.Gets(tc);
                oFabricQCGrades = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricQCGrade oFabricQCGrade = new FabricQCGrade();
                oFabricQCGrade.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricQCGrades;
        }
        public List<FabricQCGrade> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricQCGrade> oFabricQCGrades = new List<FabricQCGrade>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricQCGradeDA.Gets(tc, sSQL);
                oFabricQCGrades = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricQCGrade", e);
                #endregion
            }
            return oFabricQCGrades;
        }

        #endregion
    }
}