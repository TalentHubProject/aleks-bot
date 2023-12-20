// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Remora.Commands.Conditions;

namespace Aleks.Business.Infrastructure.Attributes;

/// <summary>
///   The attribute to use to mark a parameter as not being a bot.
/// </summary>
public class NonSelfActionableAttribute
    : ConditionAttribute
{
}