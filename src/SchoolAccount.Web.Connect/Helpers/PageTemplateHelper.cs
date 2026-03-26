namespace SchoolAccount.Web.Connect.Helpers;

internal static class PageTemplateHelper
{
    internal static string GetMojScriptAssetUrl()
    {
        return "/lib/moj-frontend/moj-frontend-8.0.0.min.js";
    }

    internal static string GetMojScriptInlineScript()
    {
        return "import * as MOJFrontend from '/lib/moj-frontend/moj-frontend-8.0.0.min.js'; MOJFrontend.initAll();";
    }

    internal static string GetMojStyleAssetUrl()
    {
        return "/lib/moj-frontend/moj-frontend-8.0.0.min.css";
    }

    internal static string GetDfeStyleAssetUrl()
    {
        return "/lib/dfe-frontend/dfefrontend-2.0.0.min.css";
    }
}
