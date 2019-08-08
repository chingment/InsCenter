using LocalS.BLL;
using LocalS.Entity;
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
    public class MerchMasterService : BaseDbContext
    {
        public string GetStatusText(bool isDisable)
        {
            string text = "";
            if (isDisable)
            {
                text = "禁用";
            }
            else
            {
                text = "正常";
            }

            return text;
        }

        public int GetStatusValue(bool isDisable)
        {
            int text = 0;
            if (isDisable)
            {
                text = 2;
            }
            else
            {
                text = 1;
            }

            return text;
        }

        public CustomJsonResult GetList(string operater, RupMerchMasterGetList rup)
        {
            var result = new CustomJsonResult();

            var query = (from u in CurrentDb.SysMerchUser
                         where (rup.UserName == null || u.UserName.Contains(rup.UserName)) &&
                         (rup.FullName == null || u.FullName.Contains(rup.FullName)) &&
                         u.IsDelete == false &&
                         u.IsMaster == true
                         select new { u.Id, u.UserName, u.FullName, u.Email, u.PhoneNumber, u.CreateTime, u.IsDelete, u.IsDisable });


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
                    UserName = item.UserName,
                    FullName = item.FullName,
                    Email = item.Email,
                    PhoneNumber = item.PhoneNumber,
                    Status = new { text = GetStatusText(item.IsDisable), value = GetStatusValue(item.IsDisable) },
                    CreateTime = item.CreateTime.ToUnifiedFormatDateTime()
                });
            }


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, Total = total, Items = olist };

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "", pageEntity);

            return result;
        }


        public List<TreeNode> GetRoleTree()
        {
            List<TreeNode> treeNodes = new List<TreeNode>();

            var sysRoles = CurrentDb.SysRole.Where(m => m.BelongSite == Enumeration.BelongSite.Merch).OrderBy(m => m.Priority).ToList();

            foreach (var sysRole in sysRoles)
            {
                treeNodes.Add(new TreeNode { Id = sysRole.Id, PId = "", Label = sysRole.Name });
            }

            return treeNodes;
        }

        public CustomJsonResult InitAdd(string operater)
        {
            var result = new CustomJsonResult();
            var ret = new RetMerchMasterInitAdd();

            ret.Roles = GetRoleTree();


            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);

            return result;
        }

        public CustomJsonResult Add(string operater, RopMerchMasterAdd rop)
        {
            var result = new CustomJsonResult();

            if (string.IsNullOrEmpty(rop.UserName))
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "用户名不能为空");
            }

            if (string.IsNullOrEmpty(rop.Password))
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "密码不能为空");
            }

            var isExistUserName = CurrentDb.SysUser.Where(m => m.UserName == rop.UserName).FirstOrDefault();
            if (isExistUserName != null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("该用户名（{0}）已被使用", rop.UserName));
            }

            using (TransactionScope ts = new TransactionScope())
            {
                var merch = new LocalS.Entity.Merch();
                merch.Id = GuidUtil.New();
                merch.Name = rop.FullName;
                merch.CreateTime = DateTime.Now;
                merch.Creator = operater;
                CurrentDb.Merch.Add(merch);


                var user = new SysMerchUser();
                user.Id = GuidUtil.New();
                user.MerchId = merch.Id;
                user.PId = GuidUtil.Empty();
                user.UserName = rop.UserName;
                user.FullName = rop.FullName;
                user.PasswordHash = PassWordHelper.HashPassword(rop.Password);
                user.Email = rop.Email;
                user.PhoneNumber = rop.PhoneNumber;
                user.BelongSite = Enumeration.BelongSite.Merch;
                user.IsDelete = false;
                user.IsDisable = false;
                user.IsMaster = true;
                user.Creator = operater;
                user.CreateTime = DateTime.Now;
                user.RegisterTime = DateTime.Now;
                user.SecurityStamp = Guid.NewGuid().ToString().Replace("-", "");
                CurrentDb.SysMerchUser.Add(user);

                if (rop.RoleIds != null)
                {
                    foreach (var roleId in rop.RoleIds)
                    {
                        if (!string.IsNullOrEmpty(roleId))
                        {
                            CurrentDb.SysUserRole.Add(new SysUserRole { Id = GuidUtil.New(), RoleId = roleId, UserId = user.Id, Creator = operater, CreateTime = DateTime.Now });
                        }
                    }
                }


                var merchOrg = new MerchOrg();
                merchOrg.Id = GuidUtil.New();
                merchOrg.MerchId = merch.Id;
                merchOrg.Name = "根组织";
                merchOrg.PId = GuidUtil.Empty();
                merchOrg.IsDelete = false;
                merchOrg.Priority = 0;
                merchOrg.Depth = 0;
                merchOrg.CreateTime = DateTime.Now;
                merchOrg.Creator = operater;
                CurrentDb.MerchOrg.Add(merchOrg);

                var merchUserOrg = new MerchUserOrg();
                merchUserOrg.Id = GuidUtil.New();
                merchUserOrg.MerchOrgId = merchOrg.Id;
                merchUserOrg.UserId = user.Id;
                merchUserOrg.CreateTime = DateTime.Now;
                merchUserOrg.Creator = operater;
                CurrentDb.MerchUserOrg.Add(merchUserOrg);

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "新建成功");

            }

            return result;
        }

        public CustomJsonResult InitEdit(string operater, string userId)
        {
            var result = new CustomJsonResult();

            var ret = new RetMerchMasterInitEdit();

            var user = CurrentDb.SysMerchUser.Where(m => m.Id == userId).FirstOrDefault();

            ret.UserId = user.Id;
            ret.UserName = user.UserName;
            ret.PhoneNumber = user.PhoneNumber;
            ret.Email = user.Email;
            ret.FullName = user.FullName;
            ret.IsDisable = user.IsDisable;

            ret.Roles = GetRoleTree();
            ret.RoleIds = (from m in CurrentDb.SysUserRole where m.UserId == user.Id select m.RoleId).ToList();

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);

            return result;
        }

        public CustomJsonResult Edit(string operater, RopMerchMasterEdit rop)
        {

            CustomJsonResult result = new CustomJsonResult();


            using (TransactionScope ts = new TransactionScope())
            {
                var user = CurrentDb.SysMerchUser.Where(m => m.Id == rop.UserId).FirstOrDefault();

                if (!string.IsNullOrEmpty(rop.Password))
                {
                    user.PasswordHash = PassWordHelper.HashPassword(rop.Password);
                }

                user.FullName = rop.FullName;
                user.Email = rop.Email;
                user.PhoneNumber = rop.PhoneNumber;
                user.IsDisable = rop.IsDisable;
                user.MendTime = DateTime.Now;
                user.Mender = operater;


                var sysUserRoles = CurrentDb.SysUserRole.Where(r => r.UserId == rop.UserId).ToList();

                foreach (var sysUserRole in sysUserRoles)
                {
                    CurrentDb.SysUserRole.Remove(sysUserRole);
                }


                if (rop.RoleIds != null)
                {
                    foreach (var roleId in rop.RoleIds)
                    {
                        if (!string.IsNullOrEmpty(roleId))
                        {
                            CurrentDb.SysUserRole.Add(new SysUserRole { Id = GuidUtil.New(), RoleId = roleId, UserId = rop.UserId, Creator = operater, CreateTime = DateTime.Now });
                        }
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
