using System;
using System.Collections.Generic;

#nullable disable

namespace secure_approve.Models.localdb
{
    public partial class RequestForm
    {
        public RequestForm()
        {
            ApproveRejectActions = new HashSet<ApproveRejectAction>();
        }

        public long RequestFormId { get; set; }
        public string RequestorEmail { get; set; }
        public string RequestorUsername { get; set; }
        public string RequestReason { get; set; }
        public decimal RequestedAmount { get; set; }
        public bool IsSubmitted { get; set; }
        public DateTime? RequestDate { get; set; }

        public virtual ICollection<ApproveRejectAction> ApproveRejectActions { get; set; }
    }
}
