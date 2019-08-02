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
    public class AdminRoleService : BaseDbContext
    {
        public CustomJsonResult GetList(string operater, RupAdminRoleGetList rup)
        {
            var result = new CustomJsonResult();

            var query = (from u in CurrentDb.SysRole
                         where (rup.Name == null || u.Name.Contains(rup.Name))
                         &&
                         u.BelongSite == Lumos.DbRelay.Enumeration.BelongSite.Admin
                         select new { u.Id, u.Name, u.Description, u.CreateTime });


            int total = query.Count();

            int pageIndex = rup.Page - 1;
            int pageSize = rup.Limit;
            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list = query.ToList();

            List<object> olist = new List<object>();

            foreach (var item in list)
            {

                olist.Add(new
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    CreateTime = item.CreateTime.ToUnifiedFormatDateTime()
                });
            }


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, Total = total, Items = olist };

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "", pageEntity);

            return result;
        }

        private List<TreeNode> GetMenuTree(string id, List<SysMenu> sysMenu)
        {
            List<TreeNode> cmbTreeList = new List<TreeNode>();

            var parentList = sysMenu.Where(t => t.PId == id).ToList();

            foreach (var item in parentList)
            {
                TreeNode treeModel = new TreeNode();
                treeModel.Id = item.Id;
                treeModel.PId = item.PId;
                treeModel.Label = item.Title;
                treeModel.Children.AddRange(GetMenuTree(treeModel.Id, sysMenu));
                cmbTreeList.Add(treeModel);
            }

            return cmbTreeList;
        }

        public List<TreeNode> GetMenuTree()
        {
            var sysMenus = CurrentDb.SysMenu.Where(m => m.BelongSite == Enumeration.BelongSite.Admin).ToList();

            var topMenu = sysMenus.Where(m => m.Dept == 0).FirstOrDefault();

            return GetMenuTree(topMenu.Id, sysMenus);
        }

        public CustomJsonResult InitAdd(string operater)
        {
            var result = new CustomJsonResult();

            var ret = new RetAdminRoleInitAdd();

            var sysMenus = CurrentDb.SysMenu.Where(m => m.BelongSite == Enumeration.BelongSite.Admin).ToList();

            var topMenu = sysMenus.Where(m => m.Dept == 0).FirstOrDefault();

            ret.Menus = GetMenuTree();
            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);

            return result;
        }

        public CustomJsonResult Add(string operater, RopAdminRoleAdd rop)
        {
            var result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var isExists = CurrentDb.SysRole.Where(m => m.Name == rop.Name && m.BelongSite == Lumos.DbRelay.Enumeration.BelongSite.Admin).FirstOrDefault();
                if (isExists != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "该名称已经存在");
                }

                var sysRole = new SysRole();
                sysRole.Id = GuidUtil.New();
                sysRole.Name = rop.Name;
                sysRole.Description = rop.Description;
                sysRole.PId = GuidUtil.Empty();
                sysRole.BelongSite = Enumeration.BelongSite.Admin;
                sysRole.Dept = 0;
                sysRole.CreateTime = DateTime.Now;
                sysRole.Creator = operater;
                CurrentDb.SysRole.Add(sysRole);

                if (rop.MenuIds != null)
                {
                    foreach (var menuId in rop.MenuIds)
                    {
                        CurrentDb.SysRoleMenu.Add(new SysRoleMenu { Id = GuidUtil.New(), RoleId = sysRole.Id, MenuId = menuId, Creator = operater, CreateTime = DateTime.Now });
                    }
                }

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }

            return result;

        }

        public CustomJsonResult InitEdit(string operater, string roleId)
        {
            var result = new CustomJsonResult();

            var ret = new RetAdminRoleInitEdit();
            var role = CurrentDb.SysRole.Where(m => m.Id == roleId).FirstOrDefault();

            ret.RoleId = role.Id;
            ret.Name = role.Name;
            ret.Description = role.Description;
            ret.Menus = GetMenuTree();

            var roleMenus = from c in CurrentDb.SysMenu
                            where
                                (from o in CurrentDb.SysRoleMenu where o.RoleId == roleId select o.MenuId).Contains(c.Id)
                            select c;


            ret.CheckedMenuIds = (from p in roleMenus select p.Id).ToList();


            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);

            return result;
        }

        public CustomJsonResult Edit(string operater, RopAdminRoleEdit rop)
        {

            CustomJsonResult result = new CustomJsonResult();


            using (TransactionScope ts = new TransactionScope())
            {
                var sysRole = CurrentDb.SysRole.Where(m => m.Id == rop.RoleId).FirstOrDefault();
                if (sysRole == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "数据为空");
                }

                sysRole.Description = rop.Description;
                sysRole.MendTime = DateTime.Now;
                sysRole.Mender = operater;

                var roleMenus = CurrentDb.SysRoleMenu.Where(r => r.RoleId == rop.RoleId).ToList();

                foreach (var roleMenu in roleMenus)
                {
                    CurrentDb.SysRoleMenu.Remove(roleMenu);
                }


                if (rop.MenuIds != null)
                {
                    foreach (var menuId in rop.MenuIds)
                    {
                        CurrentDb.SysRoleMenu.Add(new SysRoleMenu { Id = GuidUtil.New(), RoleId = rop.RoleId, MenuId = menuId, Creator = operater, CreateTime = DateTime.Now });
                    }
                }

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }

            return result;

        }
    }
}
