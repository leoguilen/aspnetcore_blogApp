namespace Medium.Core.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public static class Authors
        {
            public const string GetAll = Base + "/authors";
            public const string Create = Base + "/authors";
            public const string Get = Base + "/authors/{authorId}";
            public const string Update = Base + "/authors/{authorId}";
            public const string Delete = Base + "/authors/{authorId}";
        }

        public static class Authentication
        {
            public const string Register = Base + "/auth/register";
            public const string Login = Base + "/auth/login";
            public const string ResetPassword = Base + "/auth/reset";
        }
    }
}
