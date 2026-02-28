using System;

namespace Sanjel.RequestManagement.Core.Entities;

/// <summary>
/// ElementTypeEnum enumeration
/// </summary>
public enum ElementTypeEnum
{
    /// <summary>
    /// Text input element
    /// </summary>
    Text = 0,

    /// <summary>
    /// Numeric input element
    /// </summary>
    Number = 1,

    /// <summary>
    /// Date input element
    /// </summary>
    Date = 2,

    /// <summary>
    /// Selection input element
    /// </summary>
    Selection = 3
}