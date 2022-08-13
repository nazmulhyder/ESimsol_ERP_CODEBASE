using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Base.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class MenuTreeNodeDA
    {
        public MenuTreeNodeDA() { }

        public static void Insert(TransactionContext tc, MenuTreeNode oMenuTreeNode)
        {
            tc.ExecuteNonQuery("INSERT INTO MenuTreeNode(MenuTreeNodeID, ParentID, MenuName, LinkText, ActionName, ControllerName)"
            + "VALUES(%n,%n,%s,%s,%s,%s)",
            oMenuTreeNode.ObjectID, oMenuTreeNode.ParentID, oMenuTreeNode.MenuName, oMenuTreeNode.LinkText, oMenuTreeNode.ActionName, oMenuTreeNode.ControllerName);
        }

        public static void Update(TransactionContext tc, MenuTreeNode oMenuTreeNode)
        {
            tc.ExecuteNonQuery("UPDATE MenuTreeNode SET     ParentID=%n,    MenuName=%s,    LinkText=%s,    ActionName=%s,  ControllerName=%s"
                                + "WHERE MenuTreeNodeID=%n", oMenuTreeNode.ParentID, oMenuTreeNode.MenuName, oMenuTreeNode.LinkText, oMenuTreeNode.ActionName, oMenuTreeNode.ControllerName, oMenuTreeNode.ObjectID);
        }

        public static void Delete(TransactionContext tc, Int64 nID)
        {
            tc.ExecuteNonQuery("DELETE FROM MenuTreeNode WHERE MenuTreeNodeID=%n", nID);
        }

        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("MenuTreeNode", "MenuTreeNodeID");
        }

        public static IDataReader Get(TransactionContext tc, Int64 nID)
        {
            return tc.ExecuteReader("SELECT * FROM MenuTreeNode WHERE MenuTreeNodeID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM MenuTreeNode");
        }
    }
}
