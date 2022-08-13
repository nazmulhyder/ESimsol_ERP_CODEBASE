using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class DyeingSolutionTempletService : MarshalByRefObject, IDyeingSolutionTempletService
    {
        #region Private functions and declaration
        private DyeingSolutionTemplet MapObject(NullHandler oReader)
        {
            DyeingSolutionTemplet oDyeingSolutionTemplet = new DyeingSolutionTemplet();
            oDyeingSolutionTemplet.DSTID = oReader.GetInt32("DSTID");
            oDyeingSolutionTemplet.DyeingSolutionID = oReader.GetInt32("DyeingSolutionID");
            oDyeingSolutionTemplet.ProcessID = oReader.GetInt32("ProcessID");
            oDyeingSolutionTemplet.ParentID = oReader.GetInt32("ParentID");
            oDyeingSolutionTemplet.IsDyesChemical = oReader.GetBoolean("IsDyesChemical");
            oDyeingSolutionTemplet.TempTime = oReader.GetString("TempTime");
            oDyeingSolutionTemplet.GL = oReader.GetDouble("GL");
            oDyeingSolutionTemplet.Percentage = oReader.GetDouble("Percentage");
            oDyeingSolutionTemplet.Note = oReader.GetString("Note");
            oDyeingSolutionTemplet.RecipeCalType = (EnumDyeingRecipeType)oReader.GetInt16("RecipeCalType");
            oDyeingSolutionTemplet.ProductType = (EnumProductNature)oReader.GetInt16("ProductType");
            oDyeingSolutionTemplet.Sequence = oReader.GetInt32("Sequence");
            //Derive
            oDyeingSolutionTemplet.DyeingSolutionName = oReader.GetString("DyeingSolutionName");
            oDyeingSolutionTemplet.ProcessName = oReader.GetString("ProcessName");
            oDyeingSolutionTemplet.DyeingSolutionType = (EnumDyeingSolutionType)oReader.GetInt16("DyeingSolutionType");
            return oDyeingSolutionTemplet;

        }

        private DyeingSolutionTemplet CreateObject(NullHandler oReader)
        {
            DyeingSolutionTemplet oDyeingSolutionTemplet = MapObject(oReader);
            return oDyeingSolutionTemplet;
        }

        private List<DyeingSolutionTemplet> CreateObjects(IDataReader oReader)
        {
            List<DyeingSolutionTemplet> oDyeingSolutionTemplet = new List<DyeingSolutionTemplet>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DyeingSolutionTemplet oItem = CreateObject(oHandler);
                oDyeingSolutionTemplet.Add(oItem);
            }
            return oDyeingSolutionTemplet;
        }

        #endregion

        #region Interface implementation
        public DyeingSolutionTempletService() { }

        public DyeingSolutionTemplet IUD(DyeingSolutionTemplet oDyeingSolutionTemplet, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = DyeingSolutionTempletDA.IUD(tc, oDyeingSolutionTemplet, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oDyeingSolutionTemplet = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oDyeingSolutionTemplet=new DyeingSolutionTemplet();
                    oDyeingSolutionTemplet.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDyeingSolutionTemplet.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oDyeingSolutionTemplet;
        }

        public DyeingSolutionTemplet Get(int nDSTID, Int64 nUserId)
        {
            DyeingSolutionTemplet oDyeingSolutionTemplet = new DyeingSolutionTemplet();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DyeingSolutionTempletDA.Get(nDSTID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDyeingSolutionTemplet = CreateObject(oReader);
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
                oDyeingSolutionTemplet.ErrorMessage = e.Message;
                #endregion
            }

            return oDyeingSolutionTemplet;
        }
        public List<DyeingSolutionTemplet> Gets(Int64 nUserID)
        {
            List<DyeingSolutionTemplet> oDyeingSolutionTemplet = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DyeingSolutionTempletDA.Gets(tc);
                oDyeingSolutionTemplet = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DyeingSolutionTemplet", e);
                #endregion
            }
            return oDyeingSolutionTemplet;
        }
        public List<DyeingSolutionTemplet> Gets(string sSQL, Int64 nUserID)
        {
            List<DyeingSolutionTemplet> oDyeingSolutionTemplet = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DyeingSolutionTempletDA.Gets(sSQL, tc);
                oDyeingSolutionTemplet = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DyeingSolutionTemplet", e);
                #endregion
            }
            return oDyeingSolutionTemplet;
        }

        public DyeingSolutionTemplet RefreshSequence(DyeingSolutionTemplet oDyeingSolutionTemplet, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oDyeingSolutionTemplet.children.Count > 0)
                {
                    foreach (DyeingSolutionTemplet oItem in oDyeingSolutionTemplet.children)
                    {
                        if (oItem.DSTID > 0 && oItem.Sequence > 0)
                        {
                            DyeingSolutionTempletDA.UpdateSequence(tc, oItem);
                        }
                    }
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDyeingSolutionTemplet = new DyeingSolutionTemplet();
                oDyeingSolutionTemplet.ErrorMessage = e.Message;
                #endregion
            }
            return oDyeingSolutionTemplet;
        }
        #endregion

    }
}
