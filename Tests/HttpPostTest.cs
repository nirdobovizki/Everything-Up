﻿/*
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
using System.Net;
using ServerSelfTest.Base;

namespace ServerSelfTest.Tests
{
    public class HttpPostTest : ITest
    {
        public string Url { get; set; }
        public string Data { get; set; }

        public bool RunTest()
        {
            try
            {
                var client = new WebClient();               
                var result = client.UploadString(Url, Data);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}