using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSolChemical.Models;


namespace ESimSolChemical.Controllers
{
    public class MenuTreeController : Controller
    {

        #region Functions
        MenuTreeNode _oMenuTreeNode = new MenuTreeNode();
        List<MenuTreeNode> _oMenuTreeNodes = new List<MenuTreeNode>();
        List<MenuTreeNode> _oFinalMenuTrees = new List<MenuTreeNode>();
        MenuTreeNodes _oMTs = new MenuTreeNodes();
        MenuTreeNodes _oFMTs = new MenuTreeNodes();
        List<MenuTreeNode> _oTempMenuNodes = new List<MenuTreeNode>();

            
        
        private void AddTreeNodes(ref MenuTreeNode oParentMenuTree)
        {
            List<MenuTreeNode> oChildNodes;
            oChildNodes = MenuTreeNode.SubMenuTree(Convert.ToInt32(oParentMenuTree.ObjectID), _oMenuTreeNodes);            
            oParentMenuTree.SubNodes = oChildNodes;          

            foreach (MenuTreeNode oItem in oChildNodes)
            {               
                MenuTreeNode oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }


        private MenuTreeNode GetRoot()
        {
            MenuTreeNode oMenuTreeNode = new MenuTreeNode();
            foreach (MenuTreeNode oItem in _oMenuTreeNodes)
            {
                if (oItem.ParentID == 0)
                {
                    return oItem;
                }
            }            
            return oMenuTreeNode;
        }


        
        #endregion

        public ActionResult MenuTree()
        {
            _oMenuTreeNodes = new List<MenuTreeNode>();
            _oMenuTreeNodes = MenuTreeNode.Gets();
            
            _oMenuTreeNode = GetRoot();
            this.AddTreeNodes(ref _oMenuTreeNode);
            return View(_oMenuTreeNode);
        }

    }
}
