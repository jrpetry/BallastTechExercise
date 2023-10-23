namespace MagazineManager.Server.Controllers.BLL
{
    internal static class ApplicationUserRoles
    {
        internal const string Teacher = "teacher";
        internal const string Student = "student";

        internal static string[] RolesArray = { Teacher, Student };

        public static string RolesString { get => string.Format("{0}, {1}", RolesArray); }
    }
}
