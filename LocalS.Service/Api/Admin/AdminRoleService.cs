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

        public List<TreeNode> GetTree(string id)
        {
            List<TreeNode> cmbTreeList = new List<TreeNode>();

            var parentList = CurrentDb.SysMenu.Where(t => t.PId == id).ToList();

            foreach (var item in parentList)
            {
                TreeNode treeModel = new TreeNode();
                treeModel.Id = item.Id;
                treeModel.PId = item.PId;
                treeModel.Label = item.Title;
                treeModel.Children.AddRange(GetTree(treeModel.Id));
                cmbTreeList.Add(treeModel);
            }

            return cmbTreeList;
        }

        public CustomJsonResult InitAdd(string operater)
        {
            var result = new CustomJsonResult();

            var ret = new RetAdminRoleInitAdd();

            ret.Menus = GetTree("10000000000000000000000000000001");
            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);

            return result;
        }

        public CustomJsonResult Add(string operater, RopAdminRoleAdd rop)
        {
            var result = new CustomJsonResult();

            var isExists = CurrentDb.SysRole.Where(m => m.Name == rop.Name && m.BelongSite == Lumos.DbRelay.Enumeration.BelongSite.Admin).FirstOrDefault();
            if (isExists != null)
            {
                return new CustomJsonResult(ResultType.Failure, "该名称已经存在");
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
            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");

        }

        public CustomJsonResult InitEdit(string operater, string roleId)
        {
            var result = new CustomJsonResult();

            var ret = new RetAdminRoleInitEdit();
            var role = CurrentDb.SysRole.Where(m => m.Id == roleId).FirstOrDefault();

            ret.RoleId = role.Id;
            ret.Name = role.Name;
            ret.Description = role.Description;

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);

            return result;
        }

        public CustomJsonResult Edit(string operater, RopAdminRoleEdit rop)
        {

            CustomJsonResult result = new CustomJsonResult();

            var sysRole = CurrentDb.SysRole.Where(m => m.Id == rop.RoleId).FirstOrDefault();
            if (sysRole == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "数据为空");
            }

            sysRole.Description = rop.Description;
            sysRole.MendTime = DateTime.Now;
            sysRole.Mender = operater;

            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");

        }
    }
}
