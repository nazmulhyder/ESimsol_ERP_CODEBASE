using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 

namespace ESimSol.Services.Services
{

    public class FabricBreakageService : MarshalByRefObject, IFabricBreakageService
    {
        #region Private functions and declaration
        private FabricBreakage MapObject(NullHandler oReader)
        {
            FabricBreakage oFabricBreakage = new FabricBreakage();
            oFabricBreakage.FBreakageID = oReader.GetInt32("FBreakageID");
            oFabricBreakage.Name = oReader.GetString("Name");
            oFabricBreakage.WeavingProcess = (EnumWeavingProcess)oReader.GetInt32("WeavingProcess");
            return oFabricBreakage;
        }

        private FabricBreakage CreateObject(NullHandler oReader)
        {
            FabricBreakage oFabricBreakage = new FabricBreakage();
            oFabricBreakage = MapObject(oReader);
            return oFabricBreakage;
        }

        private List<FabricBreakage> CreateObjects(IDataReader oReader)
        {
            List<FabricBreakage> oFabricBreakage = new List<FabricBreakage>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricBreakage oItem = CreateObject(oHandler);
                oFabricBreakage.Add(oItem);
            }
            return oFabricBreakage;
        }

        #endregion

        #region Interface implementation
        public FabricBreakageService() { }

        public FabricBreakage Save(FabricBreakage oFabricBreakage, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricBreakage.FBreakageID <= 0)
                {

                    reader = FabricBreakageDA.InsertUpdate(tc, oFabricBreakage, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricBreakageDA.InsertUpdate(tc, oFabricBreakage, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBreakage = new FabricBreakage();
                    oFabricBreakage = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFabricBreakage = new FabricBreakage();
                oFabricBreakage.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricBreakage;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricBreakage oFabricBreakage = new FabricBreakage();
                oFabricBreakage.FBreakageID = id;
                DBTableReferenceDA.HasReference(tc, "FabricBreakage", id);
                FabricBreakageDA.Delete(tc, oFabricBreakage, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete FabricBreakage. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public FabricBreakage Get(int id, Int64 nUserId)
        {
            FabricBreakage oAccountHead = new FabricBreakage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricBreakageDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricBreakage", e);
                #endregion
            }

            return oAccountHead;
        }

        
        public List<FabricBreakage> Gets(int eProcess, Int64 nUserID)
        {
            List<FabricBreakage> oFabricBreakage = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBreakageDA.Gets(eProcess, tc);
                oFabricBreakage = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricBreakage", e);
                #endregion
            }

            return oFabricBreakage;
        }

        public List<FabricBreakage> Gets(Int64 nUserID)
        {
            List<FabricBreakage> oFabricBreakage = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBreakageDA.Gets( tc);
                oFabricBreakage = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricBreakage", e);
                #endregion
            }

            return oFabricBreakage;
        }
        public List<FabricBreakage> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricBreakage> oFabricBreakage = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBreakageDA.Gets(tc, sSQL);
                oFabricBreakage = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricBreakage", e);
                #endregion
            }

            return oFabricBreakage;
        }

        #endregion
    }   

}
