using LocalS.BLL;
using LocalS.Service.UI;
using Lumos;
using Lumos.DbRelay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace LocalS.Service.Api.Admin
{
    public class SysMenuService : BaseDbContext
    {
        private List<TreeNode> GetOrgTree(string id, List<SysMenu> sysMenus)
        {
            List<TreeNode> treeNodes = new List<TreeNode>();

            var p_sysMenus = sysMenus.Where(t => t.PId == id).ToList();

            foreach (var p_sysMenu in p_sysMenus)
            {
                TreeNode treeNode = new TreeNode();
                treeNode.Id = p_sysMenu.Id;
                treeNode.PId = p_sysMenu.PId;
                treeNode.Label = p_sysMenu.Name;
                treeNode.Description = p_sysMenu.Description;
                if (p_sysMenu.Depth == 0)
                {
                    treeNode.ExtAttr = new { CanDelete = false };
                }
                else
                {
                    treeNode.ExtAttr = new { CanDelete = true };
                }

                treeNode.Children.AddRange(GetOrgTree(p_sysMenu.Id, sysMenus));
                treeNodes.Add(treeNode);
            }

            return treeNodes;
        }

        public CustomJsonResult GetList(string operater, Enumeration.BelongSite belongSite, RupSysMenuGetList rup)
        {
            var result = new CustomJsonResult();

            var sysMenus = CurrentDb.SysMenu.Where(m => m.BelongSite == belongSite).OrderBy(m => m.Priority).ToList();

            var topMenu = sysMenus.Where(m => m.Depth == 0).FirstOrDefault();

            var menuTree = GetOrgTree(topMenu.PId, sysMenus);

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "", menuTree);

            return result;

        }

        public CustomJsonResult InitAdd(string operater, Enumeration.BelongSite belongSite, string pMenuId)
        {
            var result = new CustomJsonResult();

            var ret = new RetSysMenuInitAdd();

            var sysMenu = CurrentDb.SysMenu.Where(m => m.Id == pMenuId).FirstOrDefault();

            if (sysMenu != null)
            {
                ret.PMenuId = sysMenu.Id;
                ret.PMenuName = sysMenu.Name;
            }

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);

            return result;
        }

        public CustomJsonResult Add(string operater, Enumeration.BelongSite belongSite, RopSysMenuAdd rop)
        {
            var result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var isExists = CurrentDb.SysMenu.Where(m => m.Name == rop.Name).FirstOrDefault();
                if (isExists != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "该名称已经存在");
                }

                var sysMenu = new SysMenu();
                sysMenu.Id = GuidUtil.New();
                sysMenu.Name = rop.Name;
                sysMenu.Description = rop.Description;
                sysMenu.PId = rop.PMenuId;
                sysMenu.BelongSite = belongSite;
                sysMenu.Depth = 0;
                sysMenu.CreateTime = DateTime.Now;
                sysMenu.Creator = operater;
                CurrentDb.SysMenu.Add(sysMenu);

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }

            return result;

        }

        public CustomJsonResult InitEdit(string operater, Enumeration.BelongSite belongSite, string orgId)
        {
            var result = new CustomJsonResult();

            var ret = new RetSysMenuInitEdit();

            var sysOrg = CurrentDb.SysOrg.Where(m => m.Id == orgId).FirstOrDefault();

            if (sysOrg != null)
            {
                ret.MenuId = sysOrg.Id;
                ret.Name = sysOrg.Name;
                ret.Description = sysOrg.Description;

                var p_sysOrg = CurrentDb.SysOrg.Where(m => m.Id == sysOrg.PId).FirstOrDefault();

                if (p_sysOrg != null)
                {
                    ret.PMenuId = p_sysOrg.Id;
                    ret.PMenuName = p_sysOrg.Name;
                }
                else
                {
                    ret.PMenuName = "/";
                }
            }



            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);

            return result;
        }

        public CustomJsonResult Edit(string operater, Enumeration.BelongSite belongSite, RopSysMenuEdit rop)
        {

            CustomJsonResult result = new CustomJsonResult();


            using (TransactionScope ts = new TransactionScope())
            {
                var sysMenu = CurrentDb.SysMenu.Where(m => m.Id == rop.MenuId).FirstOrDefault();
                if (sysMenu == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "数据为空");
                }
                sysMenu.Name = rop.Name;
                sysMenu.Description = rop.Description;
                sysMenu.MendTime = DateTime.Now;
                sysMenu.Mender = operater;


                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }

            return result;

        }
    }
}
