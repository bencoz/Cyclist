using System;
using System.Collections.Generic;
using System.Text;

namespace Cyclist.logic.usernetwork.report
{
    class Report
    {
        public enum ReportDescription
        {
            BLOCKED,
            POLICE,
            DISRUPTION,
            UNKNOWN_DANGER
        };

        public String UserID { get; set; }
        public String ReportId { get; set; }
        public ReportDescription Description { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
