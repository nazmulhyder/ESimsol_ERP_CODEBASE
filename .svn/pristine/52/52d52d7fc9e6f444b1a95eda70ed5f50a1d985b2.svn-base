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

    public class DevelopmentYarnOptionService : MarshalByRefObject, IDevelopmentYarnOptionService
    {
        #region Private functions and declaration
        private DevelopmentYarnOption MapObject(NullHandler oReader)
        {
            DevelopmentYarnOption oDevelopmentYarnOption = new DevelopmentYarnOption();

            oDevelopmentYarnOption.DevelopmentYarnOptionID = oReader.GetInt32("DevelopmentYarnOptionID");
            oDevelopmentYarnOption.DevelopmentRecapID = oReader.GetInt32("DevelopmentRecapID");
            oDevelopmentYarnOption.YarnCategoryID = oReader.GetInt32("YarnCategoryID");
            oDevelopmentYarnOption.YarnPly = oReader.GetString("YarnPly");
            oDevelopmentYarnOption.Note = oReader.GetString("Note");
            oDevelopmentYarnOption.ProductCode = oReader.GetString("ProductCode");
            oDevelopmentYarnOption.ProductName = oReader.GetString("ProductName");
            return oDevelopmentYarnOption;
        }

        private DevelopmentYarnOption CreateObject(NullHandler oReader)
        {
            DevelopmentYarnOption oDevelopmentYarnOption = new DevelopmentYarnOption();
            oDevelopmentYarnOption = MapObject(oReader);
            return oDevelopmentYarnOption;
        }

        private List<DevelopmentYarnOption> CreateObjects(IDataReader oReader)
        {
            List<DevelopmentYarnOption> oDevelopmentYarnOption = new List<DevelopmentYarnOption>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DevelopmentYarnOption oItem = CreateObject(oHandler);
                oDevelopmentYarnOption.Add(oItem);
            }
            return oDevelopmentYarnOption;
        }

        #endregion

        #region Interface implementation
        public DevelopmentYarnOptionService() { }

        public DevelopmentYarnOption Save(DevelopmentYarnOption oDevelopmentYarnOption, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<DevelopmentYarnOption> _oDevelopmentYarnOptions = new List<DevelopmentYarnOption>();
            oDevelopmentYarnOption.ErrorMessage = "";
            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = DevelopmentYarnOptionDA.InsertUpdate(tc, oDevelopmentYarnOption, EnumDBOperation.Update, nUserID, "");
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDevelopmentYarnOption = new DevelopmentYarnOption();
                    oDevelopmentYarnOption = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDevelopmentYarnOption.ErrorMessage = e.Message;
                #endregion
            }
            return oDevelopmentYarnOption;
        }


        public DevelopmentYarnOption Get(int DevelopmentYarnOptionID, Int64 nUserId)
        {
            DevelopmentYarnOption oAccountHead = new DevelopmentYarnOption();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DevelopmentYarnOptionDA.Get(tc, DevelopmentYarnOptionID);
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
                throw new ServiceException("Failed to Get DevelopmentYarnOption", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<DevelopmentYarnOption> Gets(int LabDipOrderID, Int64 nUserID)
        {
            List<DevelopmentYarnOption> oDevelopmentYarnOption = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DevelopmentYarnOptionDA.Gets(LabDipOrderID, tc);
                oDevelopmentYarnOption = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DevelopmentYarnOption", e);
                #endregion
            }

            return oDevelopmentYarnOption;
        }

        public List<DevelopmentYarnOption> Gets(string sSQL, Int64 nUserID)
        {
            List<DevelopmentYarnOption> oDevelopmentYarnOption = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DevelopmentYarnOptionDA.Gets(tc, sSQL);
                oDevelopmentYarnOption = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DevelopmentYarnOption", e);
                #endregion
            }

            return oDevelopmentYarnOption;
        }


        public List<DevelopmentYarnOption> Gets_Report(int id, Int64 nUserID)
        {
            List<DevelopmentYarnOption> oDevelopmentYarnOption = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DevelopmentYarnOptionDA.Gets_Report(tc, id);
                oDevelopmentYarnOption = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Development Recap", e);
                #endregion
            }

            return oDevelopmentYarnOption;
        }
        #endregion
    }
    
    

}
