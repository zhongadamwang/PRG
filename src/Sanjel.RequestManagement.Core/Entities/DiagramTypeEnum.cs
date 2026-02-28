using System;

namespace Sanjel.RequestManagement.Core.Entities;

/// <summary>
/// DiagramTypeEnum enumeration
/// </summary>
public enum DiagramTypeEnum
{
    /// <summary>
    /// Process flow diagram
    /// </summary>
    ProcessFlow = 0,

    /// <summary>
    /// P&ID diagram
    /// </summary>
    PipingInstrumentation = 1,

    /// <summary>
    /// Schematic diagram
    /// </summary>
    Schematic = 2,

    /// <summary>
    /// Layout diagram
    /// </summary>
    Layout = 3
}