namespace TaskManagerAPI.Helpers
{
    public static class StatusHelper
    {
        // Dictionary<int, string> или List<(int, string)>
        public static readonly Dictionary<int, string> Statuses = new Dictionary<int, string>
    {
        { 1, "New" },
        { 2, "In Progress" },
        { 3, "Done" },
        { 4, "On Hold" }
    };
    }

}
