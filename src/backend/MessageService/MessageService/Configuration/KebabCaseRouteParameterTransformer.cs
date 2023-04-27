using System.Text.RegularExpressions;

namespace MessageService.Configuration
{
    public class KebabCaseRouteParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value)
        {
            var route = value?.ToString();

            return route == null
                ? string.Empty
                : Regex.Replace(route, "([a-z])([A-Z])", "$1-$2").ToLower();
        }
    }
}
