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
    public class DyeingSolutionService : MarshalByRefObject, IDyeingSolutionService
    {
        #region Private functions and declaration
        private DyeingSolution MapObject(NullHandler oReader)
        {
            DyeingSolution oDyeingSolution = new DyeingSolution();
            oDyeingSolution.DyeingSolutionID = oReader.GetInt32("DyeingSolutionID");
            oDyeingSolution.Code = oReader.GetString("Code");
            oDyeingSolution.DyeingSolutionType = (EnumDyeingSolutionType)oReader.GetInt16("DyeingSolutionType");
            oDyeingSolution.Name = oReader.GetString("Name");
            oDyeingSolution.Description = oReader.GetString("Description");
            oDyeingSolution.AdviseBy = oReader.GetString("AdviseBy");
            return oDyeingSolution;

            
        }

        private DyeingSolution CreateObject(NullHandler oReader)
        {
            DyeingSolution oDyeingSolution = MapObject(oReader);
            return oDyeingSolution;
        }

        private List<DyeingSolution> CreateObjects(IDataReader oReader)
        {
            List<DyeingSolution> oDyeingSolution = new List<DyeingSolution>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DyeingSolution oItem = CreateObject(oHandler);
                oDyeingSolution.Add(oItem);
            }
            return oDyeingSolution;
        }

        #endregion

        #region Interface implementation
        public DyeingSolutionService() { }

        public DyeingSolution IUD(DyeingSolution oDyeingSolution, int nDBOperation, Int64 nUserID)
        {

           
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = DyeingSolutionDA.IUD(tc, oDyeingSolution, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oDyeingSolution = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oDyeingSolution = new DyeingSolution();
                    oDyeingSolution.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDyeingSolution.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oDyeingSolution;
        }

        public DyeingSolution Copy(int nDyeingSolutionID, Int64 nUserID)
        {
            TransactionContext tc = null;
            DyeingSolution oDyeingSolution = new DyeingSolution();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = DyeingSolutionDA.Copy(tc, nDyeingSolutionID, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDyeingSolution = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDyeingSolution = new DyeingSolution();
                oDyeingSolution.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oDyeingSolution;
        }
        public DyeingSolution Get(int nDyeingSolutionID, Int64 nUserId)
        {
            DyeingSolution oDyeingSolution = new DyeingSolution();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DyeingSolutionDA.Get(nDyeingSolutionID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDyeingSolution = CreateObject(oReader);
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
                oDyeingSolution.ErrorMessage = e.Message;
                #endregion
            }

            return oDyeingSolution;
        }
        public List<DyeingSolution> Gets(Int64 nUserID)
        {
            List<DyeingSolution> oDyeingSolution = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DyeingSolutionDA.Gets(tc);
                oDyeingSolution = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DyeingSolution", e);
                #endregion
            }
            return oDyeingSolution;
        }

        public List<DyeingSolution> Gets(string sSQL, Int64 nUserID)
        {
            List<DyeingSolution> oDyeingSolution = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DyeingSolutionDA.Gets(sSQL, tc);
                oDyeingSolution = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DyeingSolution", e);
                #endregion
            }
            return oDyeingSolution;
        }


        #endregion
        
    }
}
