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
    public class AdminOrgService : BaseDbContext
    {
        private List<TreeNode> GetOrgTree(string id, List<SysOrganization> sysOrganizations)
        {
            List<TreeNode> cmbTreeList = new List<TreeNode>();

            var parentList = sysOrganizations.Where(t => t.PId == id).ToList();

            foreach (var item in parentList)
            {
                TreeNode treeModel = new TreeNode();
                treeModel.Id = item.Id;
                treeModel.PId = item.PId;
                treeModel.Label = item.Name;
                treeModel.Children.AddRange(GetOrgTree(treeModel.Id, sysOrganizations));
                cmbTreeList.Add(treeModel);
            }

            return cmbTreeList;
        }

        public CustomJsonResult GetList(string operater, RupAdminOrgGetList rup)
        {
            var result = new CustomJsonResult();

            var sysOrgs = CurrentDb.SysOrganization.ToList();

            var topOrg = sysOrgs.Where(m => m.Dept == 0).FirstOrDefault();

            var tree = GetOrgTree(topOrg.Id, sysOrgs);

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "", tree);

            return result;

        }

        public CustomJsonResult InitAdd(string operater)
        {
            var result = new CustomJsonResult();

            return result;
        }

        public CustomJsonResult Add(string operater, RopAdminOrgAdd rop)
        {
            var result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var isExists = CurrentDb.SysOrganization.Where(m => m.Name == rop.Name).FirstOrDefault();
                if (isExists != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "该名称已经存在");
                }

                var sysOrganization = new SysOrganization();
                sysOrganization.Id = GuidUtil.New();
                sysOrganization.Name = rop.Name;
                sysOrganization.Description = rop.Description;
                sysOrganization.PId = rop.PId;
                sysOrganization.Dept = 0;
                sysOrganization.CreateTime = DateTime.Now;
                sysOrganization.Creator = operater;
                CurrentDb.SysOrganization.Add(sysOrganization);

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }

            return result;

        }

        public CustomJsonResult InitEdit(string operater, string roleId)
        {
            var result = new CustomJsonResult();

 
            return result;
        }

        public CustomJsonResult Edit(string operater, RopAdminOrgEdit rop)
        {

            CustomJsonResult result = new CustomJsonResult();


            using (TransactionScope ts = new TransactionScope())
            {
                var sysOrganization = CurrentDb.SysOrganization.Where(m => m.Id == rop.OrganizationId).FirstOrDefault();
                if (sysOrganization == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "数据为空");
                }

                sysOrganization.Description = rop.Description;
                sysOrganization.MendTime = DateTime.Now;
                sysOrganization.Mender = operater;


                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }

            return result;

        }
    }
}
