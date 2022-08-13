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
    public class EmployeeCardService : MarshalByRefObject, IEmployeeCardService
    {
        #region Private functions and declaration
        private EmployeeCard MapObject(NullHandler oReader)
        {
            EmployeeCard oEmployeeCard = new EmployeeCard();
            oEmployeeCard.EmployeeCardID = oReader.GetInt32("EmployeeCardID");
            oEmployeeCard.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeCard.EmployeeCardStatus = (EnumEmployeeCardStatus)oReader.GetInt32("EmployeeCardStatus");
            oEmployeeCard.IssueDate = oReader.GetDateTime("IssueDate");
            oEmployeeCard.ExpireDate = oReader.GetDateTime("ExpireDate");
            oEmployeeCard.IsActive = oReader.GetBoolean("IsActive");
           //derive
            oEmployeeCard.EmployeeNameCode = oReader.GetString("EmployeeNameCode");
            oEmployeeCard.DepartmentName = oReader.GetString("DepartmentName");
            oEmployeeCard.DesignationName = oReader.GetString("DesignationName");
            return oEmployeeCard;
        }

        private EmployeeCard CreateObject(NullHandler oReader)
        {
            EmployeeCard oEmployeeCard = MapObject(oReader);
            return oEmployeeCard;
        }

        private List<EmployeeCard> CreateObjects(IDataReader oReader)
        {
            List<EmployeeCard> oEmployeeCard = new List<EmployeeCard>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeCard oItem = CreateObject(oHandler);
                oEmployeeCard.Add(oItem);
            }
            return oEmployeeCard;
        }

        #endregion

        #region Interface implementation
        public EmployeeCardService() { }

        public EmployeeCard IUD(EmployeeCard oEmployeeCard,  Int64 nUserID)
        {
            string[] sEmpIDs;
            sEmpIDs = oEmployeeCard.ErrorMessage.Split(',');
            oEmployeeCard.ErrorMessage = "";

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                foreach (string sEmpID in sEmpIDs)
                {
                    int EmployeeID = Convert.ToInt32(sEmpID);
                    oEmployeeCard.EmployeeID = EmployeeID;

                    IDataReader reader;
                    reader = EmployeeCardDA.IUD(tc, oEmployeeCard, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeCard.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
              
                #endregion
            }
            return oEmployeeCard;
        }


        public EmployeeCard Get(int nEmployeeCardID, Int64 nUserId)
        {
            EmployeeCard oEmployeeCard = new EmployeeCard();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeCardDA.Get(nEmployeeCardID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeCard = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeCard", e);
                oEmployeeCard.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeCard;
        }

        public EmployeeCard Get(string sSql, Int64 nUserId)
        {
            EmployeeCard oEmployeeCard = new EmployeeCard();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeCardDA.Get(sSql, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeCard = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeCard", e);
                oEmployeeCard.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeCard;
        }

        public List<EmployeeCard> Gets(Int64 nUserID)
        {
            List<EmployeeCard> oEmployeeCard = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeCardDA.Gets(tc);
                oEmployeeCard = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_EmployeeCard", e);
                #endregion
            }
            return oEmployeeCard;
        }

        public List<EmployeeCard> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeCard> oEmployeeCard = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeCardDA.Gets(sSQL, tc);
                oEmployeeCard = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_EmployeeCard", e);
                #endregion
            }
            return oEmployeeCard;
        }


        #endregion

    }
}
