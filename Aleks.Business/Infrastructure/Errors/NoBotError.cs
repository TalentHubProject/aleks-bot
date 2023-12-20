// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Remora.Results;

namespace Aleks.Business.Infrastructure.Errors;

/// <summary>
///   The error to use when the user is a bot.
/// </summary>
/// <param name="Message"></param>
public sealed record NoBotError(string Message)
    : ResultError(Message);