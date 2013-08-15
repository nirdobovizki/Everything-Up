<%--
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
 --%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Collections.Generic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Everything Up? Self Test
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<table>
<tr>
<th>&nbsp;</th>
<th style="width:90px;">R.Time (ms)</th>
<th>Check</th>
</tr>
<tr>
</tr>
<% int i = 0;
   foreach (var current in (IEnumerable<string>) ViewData["Tests"])
   { %>

   <tr>
   <td id="Status<%= i %>"><div class="Wait">&nbsp;</div></td>
   <td id="Time<%= i %>">N/A</td>
   <td><%= current %></td>
   </tr>

 <%++i;
   } %>
</table>

<input type="hidden" id="testCount" value="<%= i %>" />

Powered by <a href="http://www.nbdtech.com/Open/EverythingUp/">Everything Up? by Nbd-Tech</a>.
</asp:Content>
