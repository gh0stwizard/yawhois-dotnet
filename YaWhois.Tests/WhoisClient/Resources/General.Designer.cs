﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace YaWhois.Tests.WhoisClient.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class General {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal General() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("YaWhois.Tests.WhoisClient.Resources.General", typeof(General).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to % [whois.apnic.net]
        ///% Whois data copyright terms    http://www.apnic.net/db/dbcopyright.html
        ///
        ///% Information related to &apos;1.0.0.0 - 1.0.0.255&apos;
        ///
        ///% Abuse contact for &apos;1.0.0.0 - 1.0.0.255&apos; is &apos;abuse@apnic.net&apos;
        ///
        ///inetnum:        1.0.0.0 - 1.0.0.255
        ///netname:        APNIC-LABS
        ///descr:          APNIC and Cloudflare DNS Resolver project
        ///descr:          Routed globally by AS13335/Cloudflare
        ///descr:          Research prefix for APNIC Labs
        ///country:        AU
        ///org:            ORG-ARAD1-AP
        ///admin-c:        AR302- [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string _1_0_0_1 {
            get {
                return ResourceManager.GetString("1.0.0.1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to % By submitting a query to RIPN&apos;s Whois Service
        ///% you agree to abide by the following terms of use:
        ///% http://www.ripn.net/about/servpol.html#3.2 (in Russian)
        ///% http://www.ripn.net/about/en/servpol.html#3.2 (in English).
        ///
        ///domain:        YA.RU
        ///nserver:       ns1.yandex.ru.
        ///nserver:       ns2.yandex.ru.
        ///state:         REGISTERED, DELEGATED, VERIFIED
        ///org:           YANDEX, LLC.
        ///registrar:     RU-CENTER-RU
        ///admin-contact: https://www.nic.ru/whois
        ///created:       1999-07-12T14:40:22Z
        ///paid-till:     202 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ya_ru {
            get {
                return ResourceManager.GetString("ya.ru", resourceCulture);
            }
        }
    }
}