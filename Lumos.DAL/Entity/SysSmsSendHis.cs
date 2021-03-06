﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Lumos.DbRelay
{
    [Table("SysSmsSendHis")]
    public class SysSmsSendHis
    {
        [Key]
        public string Id { get; set; }

        [MaxLength(128)]
        public string ApiName { get; set; }

        [MaxLength(512)]
        public string TemplateParams { get; set; }

        [MaxLength(128)]
        public string TemplateCode { get; set; }

        [MaxLength(2048)]
        public string FailureReason { get; set; }

        [MaxLength(128)]
        public string Phone { get; set; }

        [MaxLength(128)]
        public string Token { get; set; }

        [MaxLength(128)]
        public string ValidCode { get; set; }

        public bool IsUse { get; set; }

        public DateTime? ExpireTime { get; set; }

        public string Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public Enumeration.SmsSendResult Result { get; set; }

    }
}
