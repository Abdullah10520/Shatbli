namespace Shatbli.Data.MetaData
{
    public static class Router
    {
        public const string root = "api";
        public const string version = "v1";
        public const string Rule = root + "/" + version + "/";

        public static class UserRouting
        {
            public const string Prefix = Rule + "users";
            public const string Register = Prefix + "/register";
            public const string Login = Prefix + "/login";
            public const string Profile = Prefix + "/profile";
        }

        public static class DesignRouting
        {
            public const string Prefix = Rule + "designs";
            public const string CreateCeramicDesign = Prefix + "/ceramic";
            public const string CreateWallPaintDesign = Prefix + "/wall-paint";
            public const string ProcessDesign = Prefix + "/{designId}/process";
            public const string GetDesignById = Prefix + "/{id}";
            public const string GetUserDesigns = Prefix;
            public const string DownloadImage = Prefix + "/{designId}/download";
        }

        public static class ProductRouting
        {
            public const string Prefix = Rule + "products";
            public const string GetCeramics = Prefix + "/ceramics";
            public const string GetPaints = Prefix + "/paints";
        }

        public static class ProductAdminRouting
        {
            public const string Prefix = Rule + "admin/products";
            public const string Create = Prefix;
            public const string Delete = Prefix + "/{id}";
        }
    }
}
