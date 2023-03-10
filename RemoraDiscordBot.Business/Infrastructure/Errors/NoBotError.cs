// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Remora.Results;

namespace RemoraDiscordBot.Core.Infrastructure.Errors;

public sealed record NoBotError(string Message)
    : ResultError(Message);