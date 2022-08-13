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
    public class MachineLiquorService : MarshalByRefObject, IMachineLiquorService
    {
        #region Private functions and declaration
        private MachineLiquor MapObject(NullHandler oReader)
        {
            MachineLiquor oMachineLiquor = new MachineLiquor();
            oMachineLiquor.MachineLiquorID = oReader.GetInt32("MachineLiquorID");
            oMachineLiquor.MachineID = oReader.GetInt32("MachineID");
            oMachineLiquor.Label = oReader.GetInt32("Label");
            oMachineLiquor.Liquor = oReader.GetDouble("Liquor");
            oMachineLiquor.LastUpdateBy = oReader.GetInt32("LastUpdateBy");
            //oMachineLiquor.LastUpdateDateTime = Convert.ToDateTime("LastUpdateDateTime");
            oMachineLiquor.LastUpdateByName = oReader.GetString("LastUpdateByName");
            return oMachineLiquor;
        }
        private MachineLiquor CreateObject(NullHandler oReader)
        {
            MachineLiquor oMachineLiquor = new MachineLiquor();
            oMachineLiquor = MapObject(oReader);
            return oMachineLiquor;
        }

        private List<MachineLiquor> CreateObjects(IDataReader oReader)
        {
            List<MachineLiquor> oMachineLiquor = new List<MachineLiquor>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MachineLiquor oItem = CreateObject(oHandler);
                oMachineLiquor.Add(oItem);
            }
            return oMachineLiquor;
        }
        #endregion
        #region Interface implementation
        public MachineLiquor Save(MachineLiquor oMachineLiquor, Int64 nUserID)
        {
            string sRefChildIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oMachineLiquor.MachineLiquorID <= 0)
                {

                    reader = MachineLiquorDA.InsertUpdate(tc, oMachineLiquor, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = MachineLiquorDA.InsertUpdate(tc, oMachineLiquor, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMachineLiquor = new MachineLiquor();
                    oMachineLiquor = CreateObject(oReader);
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
                    oMachineLiquor = new MachineLiquor();
                    oMachineLiquor.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oMachineLiquor;
        }

        public List<MachineLiquor> SaveList(List<MachineLiquor> oMachineLiquors, Int64 nUserID)
        {
            MachineLiquor oMachineLiquor = new MachineLiquor();
            List<MachineLiquor> oReturnMachineLiquors = new List<MachineLiquor>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (MachineLiquor oTempMachineLiquor in oMachineLiquors)
                {
                    #region MachineLiquor Part
                    IDataReader reader;
                    if (oTempMachineLiquor.MachineLiquorID <= 0)
                    {

                        reader = MachineLiquorDA.InsertUpdate(tc, oTempMachineLiquor, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {

                        reader = MachineLiquorDA.InsertUpdate(tc, oTempMachineLiquor, EnumDBOperation.Update, nUserID);

                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oMachineLiquor = new MachineLiquor();
                        oMachineLiquor = CreateObject(oReader);
                    }
                    reader.Close();
                    #endregion
                    oReturnMachineLiquors.Add(oMachineLiquor);
                }
                tc.End();

            }
            catch (Exception e)
            {
                if (tc != null)
                {
                    tc.HandleError();
                    oMachineLiquor = new MachineLiquor();
                    oMachineLiquor.ErrorMessage = e.Message;
                    oReturnMachineLiquors = new List<MachineLiquor>();
                    oReturnMachineLiquors.Add(oMachineLiquor);
                }
            }
            return oReturnMachineLiquors;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                MachineLiquor oMachineLiquor = new MachineLiquor();
                oMachineLiquor.MachineLiquorID = id;
                DBTableReferenceDA.HasReference(tc, "MachineLiquor", id);
                MachineLiquorDA.Delete(tc, oMachineLiquor, EnumDBOperation.Delete, nUserId);
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
        public MachineLiquor Get(int id, Int64 nUserId)
        {
            MachineLiquor oMachineLiquor = new MachineLiquor();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = MachineLiquorDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMachineLiquor = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get MachineLiquor", e);
                #endregion
            }
            return oMachineLiquor;
        }
        public List<MachineLiquor> Gets(Int64 nUserID)
        {
            List<MachineLiquor> oMachineLiquors = new List<MachineLiquor>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MachineLiquorDA.Gets(tc);
                oMachineLiquors = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                MachineLiquor oMachineLiquor = new MachineLiquor();
                oMachineLiquor.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oMachineLiquors;
        }
        public List<MachineLiquor> Gets(string sSQL, Int64 nUserID)
        {
            List<MachineLiquor> oMachineLiquors = new List<MachineLiquor>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MachineLiquorDA.Gets(tc, sSQL);
                oMachineLiquors = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MachineLiquor", e);
                #endregion
            }
            return oMachineLiquors;
        }

        #endregion
    }
}
