// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectFactory.cs" company="Silverseed.de">
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
  using System.Collections.Generic;

  public class ObjectFactory
  {
    private static ObjectFactory objectFactory;

    public static ObjectFactory Instance
    {
      get
      {
        if (objectFactory == null)
        {
          objectFactory = new ObjectFactory();
          objectFactory.ObjectStack = new Stack<object>();
        }

        return objectFactory;
      }
    }

    public Stack<object> ObjectStack { get; private set; }
  }
}
