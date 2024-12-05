using Microsoft.AspNetCore.Mvc;

namespace Server.Extensions;

public static class ControllerBaseExtensions
{
    public static string GetControllerName(this ControllerBase controllerBase)
    {
        var type = controllerBase.GetType();
        return type.Name.Replace("Controller", string.Empty);
    }
}
