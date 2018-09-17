namespace Sales01.Common.Models
{
    public class RolesHelper
    {
        public const string ADMIN = "Administrator";
        public const string PowerUser = "PowerUser";
        public const string ASSVIEWER = "Assistant";
        public const string CUSVIEWER = "Customer";
        public const string SUPPLIER = "Supplier";

        public const string ADMIN_OR_VIEWER_OR_ASSVIEWER = ADMIN + "," + PowerUser + "," + ASSVIEWER;
        public const string ADMIN_OR_PowerUser = ADMIN + "," + PowerUser;
        public const string ADMIN_OR_ASSVIEWER = ADMIN + "," + ASSVIEWER;
        public const string ADMIN_OR_VIEWER = ADMIN + "," + PowerUser;
        public const string VIEWER_OR_ASSVIEWER = PowerUser + "," + ASSVIEWER;
        public const string VIEWER_OR_CUSVIEWER = PowerUser + "," + CUSVIEWER;
        public const string ADMIN_OR_VIEWER_OR_CUSVIEWER_OR_ASSVIEWER = ADMIN + "," + PowerUser + "," + ASSVIEWER + "," + CUSVIEWER;
    }
}
