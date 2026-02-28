using System;

namespace Sanjel.RequestManagement.Core.Entities;

/// <summary>
/// ReviewStatusEnum enumeration
/// </summary>
public enum ReviewStatusEnum
{
    /// <summary>
    /// Review not started
    /// </summary>
    NotStarted = 0,

    /// <summary>
    /// Review in progress
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// Review completed
    /// </summary>
    Completed = 2,

    /// <summary>
    /// Review approved
    /// </summary>
    Approved = 3,

    /// <summary>
    /// Review rejected
    /// </summary>
    Rejected = 4
}