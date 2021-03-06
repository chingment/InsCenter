﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.DbRelay
{
    [Table("SysRolePermission")]
    public class SysRolePermission
    {
        public string Id { get; set; }
        [Key]
        [Column(Order = 1)]
        public string RoleId { get; set; }

        /// <summary>
        /// 权限ID
        /// </summary>
        [Key]
        [Column(Order = 2)]
        [MaxLength(128)]
        public string PermissionId { get; set; }

        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
