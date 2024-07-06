using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Common.Persistence.Constants;

/// <summary>
/// Contains the configuration constants of properties.
/// </summary>
public static class PropertyConstants
{
    /// <summary>
    /// Maximum length of the user's ID property.
    /// </summary>
    /// <remarks>
    /// Based on the maximum length of the ID property used by Identity.
    /// </remarks>
    public const int UserIdMaxLength = 450;

    /// <summary>
    /// Maximum length of the URL path property.
    /// </summary>
    public const int UrlPathMaxLength = 2048;

    /// <summary>
    /// Maximum length of the URL prefix property.
    /// </summary>
    public const int UrlPrefixMaxLength = 255;
}
