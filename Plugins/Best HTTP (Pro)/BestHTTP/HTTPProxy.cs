#if !BESTHTTP_DISABLE_PROXY

using System;
using BestHTTP.Authentication;

namespace BestHTTP
{
    public sealed class HTTPProxy
    {
        /// <summary>
        /// Address of the proxy server. It has to be in the http://proxyaddress:port form.
        /// </summary>
        public Uri Address { get; set; }

        /// <summary>
        /// Credentials of the proxy
        /// </summary>
        public Credentials Credentials { get; set; }

        /// <summary>
        /// True if the proxy can act as a transparent proxy
        /// </summary>
        public bool IsTransparent { get; set; }

        /// <summary>
        /// Some non-transparent proxies are except only the path and query of the request uri. Default value is true
        /// </summary>
        public bool SendWholeUri { get; set; }

        /// <summary>
        /// Regardless of the value of IsTransparent, for secure protocols(HTTPS://, WSS://) the plugin will use the proxy as an explicit proxy(will issue a CONNECT request to the proxy)
        /// </summary>
        public bool NonTransparentForHTTPS { get; set; }

        public HTTPProxy(Uri address)
            :this(address, null, false)
        {}

        public HTTPProxy(Uri address, Credentials credentials)
            :this(address, credentials, false)
        {}

        public HTTPProxy(Uri address, Credentials credentials, bool isTransparent)
            :this(address, credentials, isTransparent, true)
        { }

        public HTTPProxy(Uri address, Credentials credentials, bool isTransparent, bool sendWholeUri)
            : this(address, credentials, isTransparent, true, true)
        { }

        public HTTPProxy(Uri address, Credentials credentials, bool isTransparent, bool sendWholeUri, bool nonTransparentForHTTPS)
        {
            this.Address = address;
            this.Credentials = credentials;
            this.IsTransparent = isTransparent;
            this.SendWholeUri = sendWholeUri;
            this.NonTransparentForHTTPS = nonTransparentForHTTPS;
        }
    }
}

#endif