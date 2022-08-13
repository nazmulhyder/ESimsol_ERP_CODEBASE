using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class FNRecipeLabService : MarshalByRefObject, IFNRecipeLabService
    {
        #region Private functions and declaration
        private FNRecipeLab MapObject(NullHandler oReader)
        {
            FNRecipeLab oFNRecipeLab = new FNRecipeLab();
            oFNRecipeLab.FNRecipeLabID = oReader.GetInt32("FNRecipeLabID");
            oFNRecipeLab.FNLDDID = oReader.GetInt32("FNLDDID");
            oFNRecipeLab.FNTPID = oReader.GetInt32("FNTPID");
            oFNRecipeLab.ProductType = (EnumProductType)oReader.GetInt16("ProductType");
            oFNRecipeLab.ProductID = oReader.GetInt32("ProductID");
            oFNRecipeLab.GL = oReader.GetDouble("GL");
            oFNRecipeLab.QtyColor = oReader.GetDouble("QtyColor");
            oFNRecipeLab.Qty = oReader.GetDouble("Qty");
            oFNRecipeLab.BathSize = oReader.GetDouble("BathSize");
            oFNRecipeLab.Note = oReader.GetString("Note");
            oFNRecipeLab.ProductTypeInInt = oReader.GetInt32("ProductType");
            oFNRecipeLab.ProductName = oReader.GetString("ProductName");
            oFNRecipeLab.ProductCode = oReader.GetString("ProductCode");
            oFNRecipeLab.FNTreatment = oReader.GetInt32("FNTreatment");
            oFNRecipeLab.FNProcess = oReader.GetString("FNProcess");
            oFNRecipeLab.PadderPressure = oReader.GetString("PadderPressure");
            oFNRecipeLab.Temp = oReader.GetString("Temp");
            oFNRecipeLab.Speed = oReader.GetString("Speed");
            oFNRecipeLab.PH = oReader.GetString("PH");
            oFNRecipeLab.Flem = oReader.GetString("Flem");
            oFNRecipeLab.CausticStrength = oReader.GetString("CausticStrength");
            oFNRecipeLab.IsProcess = oReader.GetBoolean("IsProcess");
            oFNRecipeLab.Code = oReader.GetString("Code");
            oFNRecipeLab.PrepareByName = oReader.GetString("PrepareByName");
            oFNRecipeLab.ShadeID = (EnumShade)oReader.GetInt16("ShadeID");


            return oFNRecipeLab;
        }

        private FNRecipeLab CreateObject(NullHandler oReader)
        {
            FNRecipeLab oFNRecipeLab = new FNRecipeLab();
            oFNRecipeLab = MapObject(oReader);
            return oFNRecipeLab;
        }

        private List<FNRecipeLab> CreateObjects(IDataReader oReader)
        {
            List<FNRecipeLab> oFNRecipeLab = new List<FNRecipeLab>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FNRecipeLab oItem = CreateObject(oHandler);
                oFNRecipeLab.Add(oItem);
            }
            return oFNRecipeLab;
        }

        #endregion

        #region Interface implementation
        public FNRecipeLabService() { }

        public FNRecipeLab Save(FNRecipeLab oFNRecipeLab, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFNRecipeLab.FNRecipeLabID <= 0)
                {
                    reader = FNRecipeLabDA.InsertUpdate(tc, oFNRecipeLab, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FNRecipeLabDA.InsertUpdate(tc, oFNRecipeLab, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNRecipeLab = new FNRecipeLab();
                    oFNRecipeLab = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save FNRecipeLab. Because of " + e.Message.Split('~')[0], e);
                #endregion
            }
            return oFNRecipeLab;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FNRecipeLab oFNRecipeLab = new FNRecipeLab();
                oFNRecipeLab.FNRecipeLabID = id;
                FNRecipeLabDA.Delete(tc, oFNRecipeLab, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public string DeleteProcess(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FNRecipeLab oFNRecipeLab = new FNRecipeLab();
                oFNRecipeLab.FNRecipeLabID = id;
                FNRecipeLabDA.Delete(tc, oFNRecipeLab, EnumDBOperation.Cancel, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        public List<FNRecipeLab> CopyShadeSave(int nFNLDDID, int nShadeID, int nShadeIDCopy, int nFNLabDipDetailID, long nUserID)
        {
            List<FNRecipeLab> oFNRecipeLabs = new List<FNRecipeLab>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNRecipeLabDA.CopyShadeSave(tc, nFNLDDID, nShadeID, nShadeIDCopy, nFNLabDipDetailID, EnumDBOperation.Approval, nUserID);
                oFNRecipeLabs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFNRecipeLabs = new List<FNRecipeLab>();
                oFNRecipeLabs.Add(new FNRecipeLab{ErrorMessage = e.Message.Split('~')[0]});
                #endregion
            }
            return oFNRecipeLabs;
        }
        public FNRecipeLab Get(int id, Int64 nUserId)
        {
            FNRecipeLab oFNRecipeLab = new FNRecipeLab();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FNRecipeLabDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNRecipeLab = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FNRecipeLab", e);
                #endregion
            }
            return oFNRecipeLab;
        }
        public List<FNRecipeLab> Gets(Int64 nUserID)
        {
            List<FNRecipeLab> oFNRecipeLabs = new List<FNRecipeLab>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNRecipeLabDA.Gets(tc);
                oFNRecipeLabs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FNRecipeLab", e);
                #endregion
            }
            return oFNRecipeLabs;
        }
        public List<FNRecipeLab> Gets(string sSQL, Int64 nUserID)
        {
            List<FNRecipeLab> oFNRecipeLabs = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNRecipeLabDA.Gets(tc, sSQL);
                oFNRecipeLabs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FNRecipeLab", e);
                #endregion
            }
            return oFNRecipeLabs;
        }
        #endregion
    }
}
