using System;

namespace Sanjel.RequestManagement.Core.Entities;

/// <summary>
/// Recipient type enumeration for notifications
/// </summary>
public enum RecipientEnum
{
    /// <summary>
    /// Engineering staff member
    /// </summary>
    Engineer = 0,

    /// <summary>
    /// Management staff member
    /// </summary>
    Manager = 1,

    /// <summary>
    /// External client
    /// </summary>
    Client = 2,

    /// <summary>
    /// System notification
    /// </summary>
    System = 3
}