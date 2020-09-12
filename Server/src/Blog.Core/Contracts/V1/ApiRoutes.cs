namespace Medium.Core.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public static class Authentication
        {
            public const string Register = Base + "/auth/register";
            public const string Login = Base + "/auth/login";
            public const string ResetPassword = Base + "/auth/reset";
        }
        public static class Authors
        {
            public const string GetAll = Base + "/authors";
            public const string Create = Base + "/authors";
            public const string Get = Base + "/authors/{authorId}";
            public const string Update = Base + "/authors/{authorId}";
            public const string Delete = Base + "/authors/{authorId}";
        }

        public static class Posts
        {
            public const string GetAll = Base + "/posts";
            public const string Create = Base + "/posts";
            public const string Get = Base + "/posts/{postId}";
            public const string Update = Base + "/posts/{postId}";
            public const string Delete = Base + "/posts/{postId}";
        }

        public static class Tags
        {
            public const string GetAll = Base + "/tags";
            public const string Create = Base + "/tags";
            public const string Get = Base + "/tags/{tagId}";
            public const string Update = Base + "/tags/{tagId}";
            public const string Delete = Base + "/tags/{tagId}";
        }
    }
}
