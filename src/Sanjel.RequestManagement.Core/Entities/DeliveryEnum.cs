using System;

namespace Sanjel.RequestManagement.Core.Entities;

/// <summary>
/// Notification delivery status enumeration
/// </summary>
public enum DeliveryEnum
{
    /// <summary>
    /// Notification pending delivery
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Notification sent successfully
    /// </summary>
    Sent = 1,

    /// <summary>
    /// Notification delivered
    /// </summary>
    Delivered = 2,

    /// <summary>
    /// Notification delivery failed
    /// </summary>
    Failed = 3
}