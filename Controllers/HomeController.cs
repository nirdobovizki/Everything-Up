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
using System.Web.Mvc;
using System.Diagnostics;
using ServerSelfTest.Base;

namespace ServerSelfTest.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Tests"] = from t in ConfigManager.Config.Tests select t.Name;
            return View();
        }
       
        public ActionResult RunTest(int testIndex)
        {
            if (ConfigManager.Config.Tests.Count > testIndex)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                var r = ConfigManager.Config.Tests[testIndex].Test.RunTest();
                sw.Stop();
                return Json(new { TestPassed = r, ElapsedMilliseconds = sw.ElapsedMilliseconds });
            }
            throw new Exception();
        }
    }
}
