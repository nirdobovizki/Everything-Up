/*
       Copyright 2011 Nir Dobovizki

       Licensed under the Apache License, Version 2.0 (the "License");
       you may not use this file except in compliance with the License.
       You may obtain a copy of the License at

         http://www.apache.org/licenses/LICENSE-2.0

       Unless required by applicable law or agreed to in writing, software
       distributed under the License is distributed on an "AS IS" BASIS,
       WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
       See the License for the specific language governing permissions and
       limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerSelfTest.Base
{
    public class ConfigManager
    {
        private static Config _savedConfig;

        // we only have one IConfigLoader implementation at this time,
        // so we can delay writing the loadr selection logic until it's needed
        private static IConfigLoader _loader = new Utils.XmlConfigLoader();

        public static Config Config
        {
            get
            {
                if (_savedConfig == null ||
                    _loader.NeedReload(_savedConfig.Tag))
                {
                    _savedConfig = _loader.Load();
                }
                return _savedConfig;
            }
        }

    }
}