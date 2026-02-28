using System;

namespace Sanjel.RequestManagement.Core.Entities;

/// <summary>
/// AvailabilityEnum enumeration
/// </summary>
public enum AvailabilityEnum
{
    /// <summary>
    /// Available for work
    /// </summary>
    Available = 0,

    /// <summary>
    /// Currently busy
    /// </summary>
    Busy = 1,

    /// <summary>
    /// Away from office
    /// </summary>
    Away = 2,

    /// <summary>
    /// Unavailable
    /// </summary>
    Unavailable = 3
}