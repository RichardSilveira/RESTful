using System.ComponentModel;

namespace RESTPizza.Domain
{
    public enum OrderStatus
    {
        [Description("Waiting Attendance")] WaitingAttendance = 1,

        [Description("Approved")] Approved,

        [Description("Rejected")] Rejected
    }
}