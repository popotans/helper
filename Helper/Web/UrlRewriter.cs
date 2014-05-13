using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Xml;
using System.Configuration;
using System.Xml.Serialization;
namespace Helper.Web
{

    [Serializable()]
    [XmlRoot("RewriterConfig")]
    public class RewriterConfiguration
    {
        // private member variables
        private RewriterRuleCollection rules;			// an instance of the RewriterRuleCollection class...

        /// <summary>
        /// GetConfig() returns an instance of the <b>RewriterConfiguration</b> class with the values populated from
        /// the Web.config file.  It uses XML deserialization to convert the XML structure in Web.config into
        /// a <b>RewriterConfiguration</b> instance.
        /// </summary>
        /// <returns>A <see cref="RewriterConfiguration"/> instance.</returns>
        public static RewriterConfiguration GetConfig()
        {
            string key = "RewriterConfig";
            //if (HttpContext.Current.Cache[key] == null)
            //    HttpContext.Current.Cache.Insert("RewriterConfig", ConfigurationSettings.GetConfig("RewriterConfig"));

            if (HttpContext.Current.Cache[key] == null)
            {
                object obj = System.Configuration.ConfigurationManager.GetSection("RewriterConfig");
                if (obj != null)
                    HttpContext.Current.Cache.Insert(key, obj);
                else
                {
                    System.Xml.XmlDocument config = new System.Xml.XmlDocument();
                    string path = "";// string.Format("{0}\\app_data\\rewrite.xml", HttpContext.Current.Server.MapPath("~/"));
                    //path = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["RewriteConfig"]);
                    path = AppDomain.CurrentDomain.BaseDirectory + "app_data\\nrewrite.config";
                    if (!System.IO.File.Exists(path)) path = AppDomain.CurrentDomain.BaseDirectory + "app_data\\nrewrite.xml";
                    config.Load(path);
                    object configObj = new RewriterConfigSerializerSectionHandler().Create(null, null, config.SelectSingleNode("RewriterConfig"));
                    HttpRuntime.Cache.Insert(key, configObj, new System.Web.Caching.CacheDependency(path));
                }
            }

            return (RewriterConfiguration)HttpContext.Current.Cache[key];
        }

        #region Public Properties
        /// <summary>
        /// A <see cref="RewriterRuleCollection"/> instance that provides access to a set of <see cref="RewriterRule"/>s.
        /// </summary>
        public RewriterRuleCollection Rules
        {
            get
            {
                return rules;
            }
            set
            {
                rules = value;
            }
        }
        #endregion
    }


    [Serializable()]
    public class RewriterRule
    {
        // private member variables...
        private string lookFor, sendTo;

        #region Public Properties
        /// <summary>
        /// Gets or sets the pattern to look for.
        /// </summary>
        /// <remarks><b>LookFor</b> is a regular expression pattern.  Therefore, you might need to escape
        /// characters in the pattern that are reserved characters in regular expression syntax (., ?, ^, $, etc.).
        /// <p />
        /// The pattern is searched for using the <b>System.Text.RegularExpression.Regex</b> class's <b>IsMatch()</b>
        /// method.  The pattern is case insensitive.</remarks>
        public string LookFor
        {
            get
            {
                return lookFor;
            }
            set
            {
                lookFor = value;
            }
        }

        /// <summary>
        /// The string to replace the pattern with, if found.
        /// </summary>
        /// <remarks>The replacement string may use grouping symbols, like $1, $2, etc.  Specifically, the
        /// <b>System.Text.RegularExpression.Regex</b> class's <b>Replace()</b> method is used to replace
        /// the match in <see cref="LookFor"/> with the value in <b>SendTo</b>.</remarks>
        public string SendTo
        {
            get
            {
                return sendTo;
            }
            set
            {
                sendTo = value;
            }
        }
        #endregion
    }

    /// <summary>
    /// The RewriterRuleCollection models a set of RewriterRules in the Web.config file.
    /// </summary>
    /// <remarks>
    /// The RewriterRuleCollection is expressed in XML as:
    /// <code>
    /// &lt;RewriterRule&gt;
    ///   &lt;LookFor&gt;<i>pattern to search for</i>&lt;/LookFor&gt;
    ///   &lt;SendTo&gt;<i>string to redirect to</i>&lt;/LookFor&gt;
    /// &lt;RewriterRule&gt;
    /// &lt;RewriterRule&gt;
    ///   &lt;LookFor&gt;<i>pattern to search for</i>&lt;/LookFor&gt;
    ///   &lt;SendTo&gt;<i>string to redirect to</i>&lt;/LookFor&gt;
    /// &lt;RewriterRule&gt;
    /// ...
    /// &lt;RewriterRule&gt;
    ///   &lt;LookFor&gt;<i>pattern to search for</i>&lt;/LookFor&gt;
    ///   &lt;SendTo&gt;<i>string to redirect to</i>&lt;/LookFor&gt;
    /// &lt;RewriterRule&gt;
    /// </code>
    /// </remarks>
    [Serializable()]
    public class RewriterRuleCollection : CollectionBase
    {
        /// <summary>
        /// Adds a new RewriterRule to the collection.
        /// </summary>
        /// <param name="r">A RewriterRule instance.</param>
        public virtual void Add(RewriterRule r)
        {
            this.InnerList.Add(r);
        }

        /// <summary>
        /// Gets or sets a RewriterRule at a specified ordinal index.
        /// </summary>
        public RewriterRule this[int index]
        {
            get
            {
                return (RewriterRule)this.InnerList[index];
            }
            set
            {
                this.InnerList[index] = value;
            }
        }
    }
    /// <summary>
    /// Deserializes the markup in Web.config into an instance of the <see cref="RewriterConfiguration"/> class.
    /// </summary>
    public class RewriterConfigSerializerSectionHandler : IConfigurationSectionHandler
    {
        /// <summary>
        /// Creates an instance of the <see cref="RewriterConfiguration"/> class.
        /// </summary>
        /// <remarks>Uses XML Serialization to deserialize the XML in the Web.config file into an
        /// <see cref="RewriterConfiguration"/> instance.</remarks>
        /// <returns>An instance of the <see cref="RewriterConfiguration"/> class.</returns>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            // Create an instance of XmlSerializer based on the RewriterConfiguration type...
            XmlSerializer ser = new XmlSerializer(typeof(RewriterConfiguration));

            // Return the Deserialized object from the Web.config XML
            return ser.Deserialize(new XmlNodeReader(section));
        }

    }
    /// <summary>
    /// The base class for module rewriting.  This class is abstract, and therefore must be derived from.
    /// </summary>
    /// <remarks>Provides the essential base functionality for a rewriter using the HttpModule approach.</remarks>
    public abstract class BaseModuleRewriter : IHttpModule
    {
        /// <summary>
        /// Executes when the module is initialized.
        /// </summary>
        /// <param name="app">A reference to the HttpApplication object processing this request.</param>
        /// <remarks>Wires up the HttpApplication's AuthorizeRequest event to the
        /// <see cref="BaseModuleRewriter_AuthorizeRequest"/> event handler.</remarks>
        public virtual void Init(HttpApplication app)
        {
            // WARNING!  This does not work with Windows authentication!
            // If you are using Windows authentication, change to app.BeginRequest
            app.AuthorizeRequest += new EventHandler(this.BaseModuleRewriter_AuthorizeRequest);
        }

        public virtual void Dispose() { }

        /// <summary>
        /// Called when the module's AuthorizeRequest event fires.
        /// </summary>
        /// <remarks>This event handler calls the <see cref="Rewrite"/> method, passing in the
        /// <b>RawUrl</b> and HttpApplication passed in via the <b>sender</b> parameter.</remarks>
        protected virtual void BaseModuleRewriter_AuthorizeRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            Rewrite(app.Request.Path, app);
        }

        /// <summary>
        /// The <b>Rewrite</b> method must be overriden.  It is where the logic for rewriting an incoming
        /// URL is performed.
        /// </summary>
        /// <param name="requestedRawUrl">The requested RawUrl.  (Includes full path and querystring.)</param>
        /// <param name="app">The HttpApplication instance.</param>
        protected abstract void Rewrite(string requestedPath, HttpApplication app);
    }

    /// <summary>
    /// Provides a rewriting HttpModule.
    /// </summary>
    public class ModuleRewriter : BaseModuleRewriter
    {
        /// <summary>
        /// This method is called during the module's BeginRequest event.
        /// </summary>
        /// <param name="requestedRawUrl">The RawUrl being requested (includes path and querystring).</param>
        /// <param name="app">The HttpApplication instance.</param>
        protected override void Rewrite(string requestedPath, System.Web.HttpApplication app)
        {
            // log information to the Trace object.
            //  app.//context.Trace.Write("ModuleRewriter", "Entering ModuleRewriter");

            // get the configuration rules
            RewriterRuleCollection rules = RewriterConfiguration.GetConfig().Rules;

            // iterate through each rule...
            for (int i = 0; i < rules.Count; i++)
            {
                // get the pattern to look for, and Resolve the Url (convert ~ into the appropriate directory)
                string lookFor = "^" + RewriterUtils.ResolveUrl(app.Context.Request.ApplicationPath, rules[i].LookFor) + "$";

                // Create a regex (note that IgnoreCase is set...)
                Regex re = new Regex(lookFor, RegexOptions.IgnoreCase);

                // See if a match is found
                if (re.IsMatch(requestedPath))
                {
                    // match found - do any replacement needed
                    string sendToUrl = RewriterUtils.ResolveUrl(app.Context.Request.ApplicationPath, re.Replace(requestedPath, rules[i].SendTo));

                    // log rewriting information to the Trace object
                    //  app.//context.Trace.Write("ModuleRewriter", "Rewriting URL to " + sendToUrl);

                    // Rewrite the URL
                    RewriterUtils.RewriteUrl(app.Context, sendToUrl);
                    break;		// exit the for loop
                }
            }

            // Log information to the Trace object
            //  app.//context.Trace.Write("ModuleRewriter", "Exiting ModuleRewriter");
        }
    }
    /// <summary>
    /// Provides an HttpHandler that performs redirection.
    /// </summary>
    /// <remarks>The RewriterFactoryHandler checks the rewriting rules, rewrites the path if needed, and then
    /// delegates the responsibility of processing the ASP.NET page to the <b>PageParser</b> class (the same one
    /// used by the <b>PageHandlerFactory</b> class).</remarks>
    public class RewriterFactoryHandler : IHttpHandlerFactory
    {
        /// <summary>
        /// GetHandler is executed by the ASP.NET pipeline after the associated HttpModules have run.  The job of
        /// GetHandler is to return an instance of an HttpHandler that can process the page.
        /// </summary>
        /// <param name="context">The HttpContext for this request.</param>
        /// <param name="requestType">The HTTP data transfer method (<b>GET</b> or <b>POST</b>)</param>
        /// <param name="url">The RawUrl of the requested resource.</param>
        /// <param name="pathTranslated">The physical path to the requested resource.</param>
        /// <returns>An instance that implements IHttpHandler; specifically, an HttpHandler instance returned
        /// by the <b>PageParser</b> class, which is the same class that the default ASP.NET PageHandlerFactory delegates
        /// to.</returns>
        public virtual IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            // log info to the Trace object...
            //     //context.Trace.Write("RewriterFactoryHandler", "Entering RewriterFactoryHandler");

            string sendToUrl = url;
            string filePath = pathTranslated;

            // get the configuration rules
            RewriterRuleCollection rules = RewriterConfiguration.GetConfig().Rules;

            // iterate through the rules
            for (int i = 0; i < rules.Count; i++)
            {
                // Get the pattern to look for (and resolve its URL)
                string lookFor = "^" + RewriterUtils.ResolveUrl(context.Request.ApplicationPath, rules[i].LookFor) + "$";

                // Create a regular expression object that ignores case...
                Regex re = new Regex(lookFor, RegexOptions.IgnoreCase);

                // Check to see if we've found a match
                if (re.IsMatch(url))
                {
                    // do any replacement needed
                    sendToUrl = RewriterUtils.ResolveUrl(context.Request.ApplicationPath, re.Replace(url, rules[i].SendTo));

                    // log info to the Trace object...
                    //context.Trace.Write("RewriterFactoryHandler", "Found match, rewriting to " + sendToUrl);

                    // Rewrite the path, getting the querystring-less url and the physical file path
                    string sendToUrlLessQString;
                    RewriterUtils.RewriteUrl(context, sendToUrl, out sendToUrlLessQString, out filePath);

                    // return a compiled version of the page
                    //context.Trace.Write("RewriterFactoryHandler", "Exiting RewriterFactoryHandler");	// log info to the Trace object...
                    return PageParser.GetCompiledPageInstance(sendToUrlLessQString, filePath, context);
                }
            }


            // if we reached this point, we didn't find a rewrite match
            //context.Trace.Write("RewriterFactoryHandler", "Exiting RewriterFactoryHandler");	// log info to the Trace object...
            return PageParser.GetCompiledPageInstance(url, filePath, context);
        }

        public virtual void ReleaseHandler(IHttpHandler handler)
        {
        }
    }


    /// <summary>
    /// Provides utility helper methods for the rewriting HttpModule and HttpHandler.
    /// </summary>
    /// <remarks>This class is marked as internal, meaning only classes in the same assembly will be
    /// able to access its methods.</remarks>
    internal class RewriterUtils
    {
        #region RewriteUrl
        /// <summary>
        /// Rewrite's a URL using <b>HttpContext.RewriteUrl()</b>.
        /// </summary>
        /// <param name="context">The HttpContext object to rewrite the URL to.</param>
        /// <param name="sendToUrl">The URL to rewrite to.</param>
        internal static void RewriteUrl(HttpContext context, string sendToUrl)
        {
            string x, y;
            RewriteUrl(context, sendToUrl, out x, out y);
        }

        /// <summary>
        /// Rewrite's a URL using <b>HttpContext.RewriteUrl()</b>.
        /// </summary>
        /// <param name="context">The HttpContext object to rewrite the URL to.</param>
        /// <param name="sendToUrl">The URL to rewrite to.</param>
        /// <param name="sendToUrlLessQString">Returns the value of sendToUrl stripped of the querystring.</param>
        /// <param name="filePath">Returns the physical file path to the requested page.</param>
        internal static void RewriteUrl(HttpContext context, string sendToUrl, out string sendToUrlLessQString, out string filePath)
        {
            // see if we need to add any extra querystring information
            if (context.Request.QueryString.Count > 0)
            {
                if (sendToUrl.IndexOf('?') != -1)
                    sendToUrl += "&" + context.Request.QueryString.ToString();
                else
                    sendToUrl += "?" + context.Request.QueryString.ToString();
            }

            // first strip the querystring, if any
            string queryString = string.Empty;
            sendToUrlLessQString = sendToUrl;
            if (sendToUrl.IndexOf('?') > 0)
            {
                sendToUrlLessQString = sendToUrl.Substring(0, sendToUrl.IndexOf('?'));
                queryString = sendToUrl.Substring(sendToUrl.IndexOf('?') + 1);
            }

            // grab the file's physical path
            filePath = string.Empty;
            filePath = context.Server.MapPath(sendToUrlLessQString);

            // rewrite the path...
            context.RewritePath(sendToUrlLessQString, string.Empty, queryString);

            // NOTE!  The above RewritePath() overload is only supported in the .NET Framework 1.1
            // If you are using .NET Framework 1.0, use the below form instead:
            // context.RewritePath(sendToUrl);
        }
        #endregion

        /// <summary>
        /// Converts a URL into one that is usable on the requesting client.
        /// </summary>
        /// <remarks>Converts ~ to the requesting application path.  Mimics the behavior of the 
        /// <b>Control.ResolveUrl()</b> method, which is often used by control developers.</remarks>
        /// <param name="appPath">The application path.</param>
        /// <param name="url">The URL, which might contain ~.</param>
        /// <returns>A resolved URL.  If the input parameter <b>url</b> contains ~, it is replaced with the
        /// value of the <b>appPath</b> parameter.</returns>
        internal static string ResolveUrl(string appPath, string url)
        {
            if (url.Length == 0 || url[0] != '~')
                return url;		// there is no ~ in the first character position, just return the url
            else
            {
                if (url.Length == 1)
                    return appPath;  // there is just the ~ in the URL, return the appPath
                if (url[1] == '/' || url[1] == '\\')
                {
                    // url looks like ~/ or ~\
                    if (appPath.Length > 1)
                        return appPath + "/" + url.Substring(2);
                    else
                        return "/" + url.Substring(2);
                }
                else
                {
                    // url looks like ~something
                    if (appPath.Length > 1)
                        return appPath + "/" + url.Substring(1);
                    else
                        return appPath + url.Substring(1);
                }
            }
        }
    }

}
