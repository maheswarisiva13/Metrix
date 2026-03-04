namespace Metrix.API.Constants;

public static class ApiRoutes
{
    private const string Base = "/api";

    public static class Setup
    {
        private const string Root = Base + "/setup";
        public const string CreateAdmin = Root + "/create-admin";
    }

    public static class Admin
    {
        private const string Root = Base + "/admin";
        public const string CreateSecurity = Root + "/create-security";
    }

    public static class Hr
    {
        public const string Root = Base + "/hr";
        public const string Dashboard = Root + "/dashboard";

        public const string Visitors = Root + "/visitors";
        public const string PendingVisitors = Root + "/visitors/pending";
        public const string RecentVisitors = Root + "/visitors/recent";

        public const string ApproveVisitor = Root + "/visitors/{id:int}/approve";
        public const string RejectVisitor = Root + "/visitors/{id:int}/reject";
    }

    public static class Security
    {
        public const string Root = Base + "/security";
        public const string ById = Root + "/{id:int}";
    }

    public static class Auth
    {
        public const string Login = Base + "/auth/login";
    }

    public static class Visitor
    {
        public const string Root = Base + "/visitor";
    }

    public static class Invitation
    {
        public const string Root = Base + "/invitations";
        public const string Send = Root + "/send";
        public const string GetAll = Root;
    }
}