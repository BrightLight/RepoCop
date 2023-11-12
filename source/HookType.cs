// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HookType.cs" company="Silverseed.de">
//    (c) 2010 Markus Hastreiter @ Silverseed.de
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Silverseed.RepoCop
{
  /// <summary>
  /// Possible type of repository hooks.
  /// </summary>
  public enum HookType
  {
    /// <summary>
    /// An undefined hook type.
    /// </summary>
    Undefined,

    /// <summary>
    /// A start commit hook.
    /// </summary>
    StartCommit,

    /// <summary>
    /// A pre-commit hook.
    /// </summary>
    PreCommit,

    /// <summary>
    /// A post-commit hook.
    /// </summary>
    PostCommit
  }
}