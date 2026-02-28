using System;

namespace Sanjel.RequestManagement.Core.Entities;

/// <summary>
/// NotificationTypeEnum enumeration
/// </summary>
public enum NotificationTypeEnum
{
    /// <summary>
    /// Email notification
    /// </summary>
    Email = 0,

    /// <summary>
    /// SMS notification
    /// </summary>
    SMS = 1,

    /// <summary>
    /// In-application notification
    /// </summary>
    InApp = 2,

    /// <summary>
    /// System notification
    /// </summary>
    System = 3
}