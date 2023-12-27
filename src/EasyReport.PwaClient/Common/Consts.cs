namespace EasyReport.PwaClient.Common;

public class Consts
{
    public const string TokenKey = "EasyReport:jwtToken";

    public class RouteMap
    {
        public const string Login = "/login";
        public const string Home = "/";
        public const string Todo = "/todo";
        public const string TodoDay = "/todo/1";
        public const string TodoWeek = "/todo/7";
        public const string TodoMonth = "/todo/30";

        public const string Config = "/config";

        public const string TodoGroup = "/todo-group";
        public const string TodoTag = "/todo-tag";
    }
}