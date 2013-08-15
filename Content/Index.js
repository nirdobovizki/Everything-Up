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


var current = -1;

function Next() {
    ++current;
    var length = parseInt($("#testCount").val());
    if (current >= length) return;
    $("#Status" + current + " div").attr("class", "Check");
    $.ajax({
        type: "POST",
        url: "Home/RunTest",
        data: { testIndex: current },
        dataType: "json",
        success: OnSuccess,
        error: OnError
    });
}

function OnSuccess(data) {
    $("#Status" + current + " div").attr("class", data.TestPassed ? "Ok" : "Fail");
    $("#Time" + current).text(data.ElapsedMilliseconds);
    Next();
}

function OnError(req, status, error) {
    $("#Status" + current + " div").attr("class", "Error");
    Next();
}

Next();