<h1>Everything Up? Server Self Test</h1>

<p>Simple Free and Open Source Server Check Tool for ASP.net</p>
<p>Can test any HTTP or HTTPS based service, mail services (SMTP and POP3), HTTP redirects and most text based network protocols, can be extended by writing new tests in C#</p>
<p>Everything Up is licensed under the Apache version 2 license, this means you can use it for free everywhere, modify it any way you wish and use it as part of other software (even commercial software).</p>
<h2>About Everything Up</h2>
<p>We have a lot of different services running on our server, we have web sites, web applications, blogs, mail and internal tools, some of them even share the same IIS web site.</p>
<p>When so many different programs are running on the same server it's easy to make a configuration change in one application that will affect other applications, after a somewhat nasty outage caused by such a changed we started using a checklist to make sure everything is up after every change - but that was time consuming and tedious - and so Everything Up was born.</p>
<p>Everything up is a very simple web application, you create a list of everything you need to test in an XML file on the server - and after you make any change to the server you just open Everything Up in your browser and it will run the tests from the XML file and show you the results.</p>
<h2>Download</h2>
<p>The latest version is 1.0, released on September 4th, 2011:</p>
<p><a href="EverythingUpBin.zip">Precompiled Binaries (19KB, zip file)</a></p>
<p><a href="EverythingUpSrc.zip">Source Code (344KB, zip file)</a></p>
<h2>Installation and System Requirements</h2>
<p>Everything Up requires IIS and The .net Framework version 3.5 on Windows operating system.</p>
<p>To install (on IIS 7.x):</p>
<ol>
  <li>just download the latest binaries above, unzip into a folder on your web site (for example, if you have just one web site and you are using the default IIS configuration, create a folder named EverythingUp under c:\inepub\wwwroot and unzip the file below into it).</li>
  <li>Open IIS Manager, go to the new folder and make it a web application (right click on the folder and select "Convert to Application").</li>
  <li>
    Make sure the application is running under .net 3.5 (IIS manager should show runtime version 2.0):<br/>
    Click on the Everything Up folder in IIS Manager, now click the "Basic Settings" link on the right side of the window, note the name of the application pool in the basic settings window and click Cancel.<br/>
    Now, in the tree on the left click "Application Pools" (at the top, right below the computer name), on the right select the application pool that Everything Up is running in, if the .net version is v2.0 everything is set up, otherwise double click on the application pool row in the table and set the .net version to 2.0.x.
  </li>
  <li>Configure your tests - Go to the folder containing Everything up, open the "bin" sub-folder and edit the Test.xml File, see "Tests Configuration" below for details on how to set up all the possible tests.</li>
</ol>
<h2>Tests Configuration</h2>
<h3>HTTP and HTTPS</h3>
<pre>
&lt;Test Type="HttpDownload"
      Name="&lt;display name&gt;"
      Url="&lt;page url&gt;"
      LookFor="&lt;text&gt;" <i>(optional)</i> /&gt;
</pre>
<p><b>Name</b> - name to display on results page</p>
<p><b>Url</b> - Url of web page to download (example: "http://www.example.com/MyPage.aspx")</p>
<p><b>LookFor</b> - Text that must appear on page (optional)</p>
<h3>SMTP</h3>
<pre>
&lt;Test Type="Smtp"
      Name="&lt;display name&gt;"
      Server="&lt;server name&gt;"
      Port="&lt;port number&gt;" <i>(optional)</i> /&gt;
</pre>
<p><b>Name</b> - name to display on results page</p>
<p><b>Server</b> - Server name (example: "mail.example.com")</p>
<p><b>Port</b> - SMTP port, use for testing alternative SMTP ports (optional)</p>
<h3>POP3</h3>
<pre>
&lt;Test Type="Pop3"
      Name="&lt;display name&gt;"
      Server="&lt;server name&gt;" /&gt;
</pre>
<p><b>Name</b> - name to display on results page</p>
<p><b>Server</b> - Server name (example: “mail.example.com")</p>
<h3>HTTP (and HTTPS) Redirects</h3>
<pre>
&lt;Test Type="Redirect"
      Name="&lt;display name&gt;"
      Url="&lt;url to test&gt;"
      Target="&lt;redirection target&gt;"
      Permanent="&lt;true|false&gt;" /&gt;
</pre>
<p><b>Name</b> - name to display on results page</p>
<p><b>Url</b> - Url of web page to download that should redirect (example: "http://www.example.com/MyPage.aspx")</p>
<p><b>Target</b> - Url that the should be redirected to</p>
<p><b>Permanent</b> - "true" if this should be a permanent redirect (301), "false" for temporary redirect (302)</p>
<h3>Any other text based TCP/IP protocol with a status code in the first line (advanced)</h3>
<p>With this option Everything up will connect to the specified machine/port and wait for one line of text (it will read everything until the first newline), than it will look for the specified text in this line.</p>
<pre>
&lt;Test Type="Smtp"
      Name="&lt;display name&gt;"
      Server="&lt;server name&gt;"
      Port="&lt;port number&gt;"
      LookFor="&lt;status text&gt;" /&gt;
</pre>
<p><b>Name</b> - name to display on results page</p>
<p><b>Server</b> - server name (example: "mail.example.com")</p>
<p><b>Post</b> - port number</p>
<p><b>LookFor</b> - Text that should appear on first line sent from the server</p>
<p>Example: you can test POP3 with the following configuration:</p>
<pre>
&lt;Test Type="TcpTextLine" Name="POP3" Server="mail.example.com" Port="110"  LookFor="+OK " /&gt;
</pre>
<h3>Writing your own tests (advanced)</h3>
<p>Each type of test is a .net class, all tests must be compiled into the ServerSelfTest.dll (the main Everything Up dll), must be in the ServerSelfTest.Tests namespace, the class name must end with “Test" and it must implement the ServerSelfTest.Models.ITest interface</p>
<p>All public properties of the test class can be set from the Tests.xml file and the RunTest method (from the ITest interface) is called to actually perform the test.</p>
<p>For example, let's look at the code complete for the RedirectTest class:</p>
<pre>
using System;
using System.Web;
using System.Net;
using ServerSelfTest.Base;

namespace ServerSelfTest.Tests
{
    public class RedirectTest : ITest
    {
        public string Url { get; set; }
        public string Target { get; set; }
        public bool Permanent { get; set; }

        public bool RunTest()
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Url);
                request.AllowAutoRedirect = false;
                var response = (HttpWebResponse)request.GetResponse();
                return (Permanent ? 
                    response.StatusCode == HttpStatusCode.MovedPermanently : 
                    response.StatusCode == HttpStatusCode.Moved) &amp;&amp;
                    response.Headers["Location"] == Target;
            }
            catch
            {
                return false;
            }

        }
    }
}  
</pre>
<p>And the XML definitions to us the class:</p>
<pre>
&lt;Test Type="Redirect" Name="example.com -&gt; www.example.com" 
   Url="http://example.com" Target="http://www.example.com" Permanent="true" /&gt;
</pre>
<p>The Tests.xml parser creates and object of the class by looking for a class with the name specified in the Type attribute (but with the “Test" suffix) and then copies the content of the Xml attributes into the class properties.</p>
<p>The Type and Name attributes are reserved and not passed to the test class.</p>
