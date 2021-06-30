using System;
using System.Collections.Generic;

#nullable disable

namespace secure_approve.Models.localdb
{
    public partial class ApproveRejectAction
    {
        public long ActionId { get; set; }
        public long RequestFormId { get; set; }
        public string RequestorEmail { get; set; }
        public string ApproverEmail { get; set; }
        public string ApproverUsername { get; set; }
        public string RequestReason { get; set; }
        public decimal RequestedAmount { get; set; }
        public DateTime? RequestDate { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsRejected { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string UserDataHash { get; set; }
        public string ApprovalRejectWithUserDataHash { get; set; }

        public virtual RequestForm RequestForm { get; set; }
    }
}
