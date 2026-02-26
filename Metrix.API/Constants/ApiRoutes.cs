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
        public const string ById = Root + "/{id:int}";
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
    }
}
