//
// Auto-generated code. Don't edit.
//

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace YaWhois.Data
{
    public partial class Assignments
    {
        /// <summary>
        /// Format: Network / Mask / Server.
        /// </summary>
        public static readonly Tuple<uint, uint, string>[] IPv4 = {
        //
        // https://www.iana.org/assignments/ipv4-recovered-address-space/ipv4-recovered-address-space-2.csv
        //
            // 43.224.0.0 - 43.231.255.255
            Tuple.Create(736100352U, 4294443008U, "whois.apnic.net"),
            // 43.236.0.0 - 43.243.255.255
            Tuple.Create(736886784U, 4294705152U, "whois.apnic.net"),
            Tuple.Create(737148928U, 4294705152U, "whois.apnic.net"),
            // 43.245.0.0 - 43.252.255.255
            Tuple.Create(737476608U, 4294901760U, "whois.apnic.net"),
            Tuple.Create(737542144U, 4294836224U, "whois.apnic.net"),
            Tuple.Create(737673216U, 4294705152U, "whois.apnic.net"),
            Tuple.Create(737935360U, 4294901760U, "whois.apnic.net"),
            // 43.254.0.0 - 43.255.255.255
            Tuple.Create(738066432U, 4294836224U, "whois.apnic.net"),
            // 45.2.0.0 - 45.3.255.255
            Tuple.Create(755105792U, 4294836224U, "whois.arin.net"),
            // 45.4.0.0 - 45.7.255.255
            Tuple.Create(755236864U, 4294705152U, "whois.lacnic.net"),
            // 45.8.0.0 - 45.15.255.255
            Tuple.Create(755499008U, 4294443008U, "whois.ripe.net"),
            // 45.16.0.0 - 45.31.255.255
            Tuple.Create(756023296U, 4293918720U, "whois.arin.net"),
            // 45.32.0.0 - 45.63.255.255
            Tuple.Create(757071872U, 4292870144U, "whois.arin.net"),
            // 45.64.0.0 - 45.65.15.255
            Tuple.Create(759169024U, 4294901760U, "whois.apnic.net"),
            Tuple.Create(759234560U, 4294963200U, "whois.apnic.net"),
            // 45.65.16.0 - 45.65.63.255
            Tuple.Create(759238656U, 4294963200U, "whois.apnic.net"),
            Tuple.Create(759242752U, 4294959104U, "whois.apnic.net"),
            // 45.65.64.0 - 45.65.127.255
            Tuple.Create(759250944U, 4294950912U, "whois.ripe.net"),
            // 45.65.128.0 - 45.65.255.255
            Tuple.Create(759267328U, 4294934528U, "whois.lacnic.net"),
            // 45.66.0.0 - 45.67.255.255
            Tuple.Create(759300096U, 4294836224U, "whois.ripe.net"),
            // 45.68.0.0 - 45.71.255.255
            Tuple.Create(759431168U, 4294705152U, "whois.lacnic.net"),
            // 45.72.0.0 - 45.79.255.255
            Tuple.Create(759693312U, 4294443008U, "whois.arin.net"),
            // 45.80.0.0 - 45.95.255.255
            Tuple.Create(760217600U, 4293918720U, "whois.ripe.net"),
            // 45.96.0.0 - 45.111.255.255
            Tuple.Create(761266176U, 4293918720U, "whois.afrinic.net"),
            // 45.112.0.0 - 45.127.255.255
            Tuple.Create(762314752U, 4293918720U, "whois.apnic.net"),
            // 45.128.0.0 - 45.159.255.255
            Tuple.Create(763363328U, 4292870144U, "whois.ripe.net"),
            // 45.160.0.0 - 45.191.255.255
            Tuple.Create(765460480U, 4292870144U, "whois.lacnic.net"),
            // 45.192.0.0 - 45.222.255.255
            Tuple.Create(767557632U, 4293918720U, "whois.afrinic.net"),
            Tuple.Create(768606208U, 4294443008U, "whois.afrinic.net"),
            Tuple.Create(769130496U, 4294705152U, "whois.afrinic.net"),
            Tuple.Create(769392640U, 4294836224U, "whois.afrinic.net"),
            Tuple.Create(769523712U, 4294901760U, "whois.afrinic.net"),
            // 45.223.0.0 - 45.223.255.255
            Tuple.Create(769589248U, 4294901760U, "whois.arin.net"),
            // 45.224.0.0 - 45.239.255.255
            Tuple.Create(769654784U, 4293918720U, "whois.lacnic.net"),
            // 45.240.0.0 - 45.247.255.255
            Tuple.Create(770703360U, 4294443008U, "whois.afrinic.net"),
            // 45.248.0.0 - 45.255.255.255
            Tuple.Create(771227648U, 4294443008U, "whois.apnic.net"),
            // 66.218.132.0 - 66.218.133.255
            Tuple.Create(1121616896U, 4294966784U, "whois.arin.net"),
            // 66.251.128.0 - 66.251.191.255
            Tuple.Create(1123778560U, 4294950912U, "whois.afrinic.net"),
            // 72.44.16.0 - 72.44.31.255
            Tuple.Create(1210847232U, 4294963200U, "whois.lacnic.net"),
            // 74.91.48.0 - 74.91.63.255
            Tuple.Create(1247490048U, 4294963200U, "whois.arin.net"),
            // 128.201.0.0 - 128.201.255.255
            Tuple.Create(2160656384U, 4294901760U, "whois.lacnic.net"),
            // 131.196.0.0 - 131.196.255.255
            Tuple.Create(2210660352U, 4294901760U, "whois.lacnic.net"),
            // 137.59.0.0 - 137.59.255.255
            Tuple.Create(2302345216U, 4294901760U, "whois.apnic.net"),
            // 139.5.0.0 - 139.5.255.255
            Tuple.Create(2332360704U, 4294901760U, "whois.apnic.net"),
            // 139.26.0.0 - 139.26.255.255
            Tuple.Create(2333736960U, 4294901760U, "whois.afrinic.net"),
            // 139.28.0.0 - 139.28.255.255
            Tuple.Create(2333868032U, 4294901760U, "whois.ripe.net"),
            // 144.48.0.0 - 144.48.255.255
            Tuple.Create(2419064832U, 4294901760U, "whois.apnic.net"),
            // 144.168.0.0 - 144.168.255.255
            Tuple.Create(2426929152U, 4294901760U, "whois.arin.net"),
            // 146.196.32.0 - 146.196.127.255
            Tuple.Create(2462326784U, 4294959104U, "whois.apnic.net"),
            Tuple.Create(2462334976U, 4294950912U, "whois.apnic.net"),
            // 146.196.128.0 - 146.196.255.255
            Tuple.Create(2462351360U, 4294934528U, "whois.afrinic.net"),
            // 147.78.0.0 - 147.78.255.255
            Tuple.Create(2471362560U, 4294901760U, "whois.ripe.net"),
            // 149.248.0.0 - 149.248.255.255
            Tuple.Create(2516058112U, 4294901760U, "whois.arin.net"),
            // 150.107.0.0 - 150.107.255.255
            Tuple.Create(2523594752U, 4294901760U, "whois.apnic.net"),
            // 150.129.0.0 - 150.129.255.255
            Tuple.Create(2525036544U, 4294901760U, "whois.apnic.net"),
            // 150.242.0.0 - 150.242.255.255
            Tuple.Create(2532442112U, 4294901760U, "whois.apnic.net"),
            // 152.89.0.0 - 152.89.255.255
            Tuple.Create(2555969536U, 4294901760U, "whois.ripe.net"),
            // 154.16.0.0 - 154.16.255.255
            Tuple.Create(2584739840U, 4294901760U, "whois.afrinic.net"),
            // 157.119.0.0 - 157.119.255.255
            Tuple.Create(2641821696U, 4294901760U, "whois.apnic.net"),
            // 160.19.0.0 - 160.19.15.255
            Tuple.Create(2685599744U, 4294963200U, "whois.arin.net"),
            // 160.19.20.0 - 160.19.23.255
            Tuple.Create(2685604864U, 4294966272U, "whois.apnic.net"),
            // 160.19.24.0 - 160.19.31.255
            Tuple.Create(2685605888U, 4294965248U, "whois.arin.net"),
            // 160.19.36.0 - 160.19.39.255
            Tuple.Create(2685608960U, 4294966272U, "whois.afrinic.net"),
            // 160.19.44.0 - 160.19.47.255
            Tuple.Create(2685611008U, 4294966272U, "whois.lacnic.net"),
            // 160.19.48.0 - 160.19.55.255
            Tuple.Create(2685612032U, 4294965248U, "whois.apnic.net"),
            // 160.19.60.0 - 160.19.63.255
            Tuple.Create(2685615104U, 4294966272U, "whois.afrinic.net"),
            // 160.19.64.0 - 160.19.67.255
            Tuple.Create(2685616128U, 4294966272U, "whois.apnic.net"),
            // 160.19.92.0 - 160.19.95.255
            Tuple.Create(2685623296U, 4294966272U, "whois.ripe.net"),
            // 160.19.96.0 - 160.19.103.255
            Tuple.Create(2685624320U, 4294965248U, "whois.afrinic.net"),
            // 160.19.104.0 - 160.19.107.255
            Tuple.Create(2685626368U, 4294966272U, "whois.arin.net"),
            // 160.19.112.0 - 160.19.143.255
            Tuple.Create(2685628416U, 4294963200U, "whois.afrinic.net"),
            Tuple.Create(2685632512U, 4294963200U, "whois.afrinic.net"),
            // 160.19.152.0 - 160.19.155.255
            Tuple.Create(2685638656U, 4294966272U, "whois.afrinic.net"),
            // 160.19.160.0 - 160.19.163.255
            Tuple.Create(2685640704U, 4294966272U, "whois.arin.net"),
            // 160.19.168.0 - 160.19.175.255
            Tuple.Create(2685642752U, 4294965248U, "whois.lacnic.net"),
            // 160.19.180.0 - 160.19.183.255
            Tuple.Create(2685645824U, 4294966272U, "whois.ripe.net"),
            // 160.19.188.0 - 160.19.191.255
            Tuple.Create(2685647872U, 4294966272U, "whois.afrinic.net"),
            // 160.19.192.0 - 160.19.199.255
            Tuple.Create(2685648896U, 4294965248U, "whois.afrinic.net"),
            // 160.19.200.0 - 160.19.203.255
            Tuple.Create(2685650944U, 4294966272U, "whois.lacnic.net"),
            // 160.19.208.0 - 160.19.223.255
            Tuple.Create(2685652992U, 4294963200U, "whois.apnic.net"),
            // 160.19.224.0 - 160.19.227.255
            Tuple.Create(2685657088U, 4294966272U, "whois.apnic.net"),
            // 160.19.232.0 - 160.19.239.255
            Tuple.Create(2685659136U, 4294965248U, "whois.afrinic.net"),
            // 160.19.240.0 - 160.19.255.255
            Tuple.Create(2685661184U, 4294963200U, "whois.lacnic.net"),
            // 160.20.0.0 - 160.20.15.255
            Tuple.Create(2685665280U, 4294963200U, "whois.apnic.net"),
            // 160.20.20.0 - 160.20.23.255
            Tuple.Create(2685670400U, 4294966272U, "whois.lacnic.net"),
            // 160.20.24.0 - 160.20.31.255
            Tuple.Create(2685671424U, 4294965248U, "whois.afrinic.net"),
            // 160.20.32.0 - 160.20.35.255
            Tuple.Create(2685673472U, 4294966272U, "whois.lacnic.net"),
            // 160.20.40.0 - 160.20.47.255
            Tuple.Create(2685675520U, 4294965248U, "whois.apnic.net"),
            // 160.20.48.0 - 160.20.63.255
            Tuple.Create(2685677568U, 4294963200U, "whois.apnic.net"),
            // 160.20.64.0 - 160.20.71.255
            Tuple.Create(2685681664U, 4294965248U, "whois.lacnic.net"),
            // 160.20.72.0 - 160.20.75.255
            Tuple.Create(2685683712U, 4294966272U, "whois.apnic.net"),
            // 160.20.80.0 - 160.20.95.255
            Tuple.Create(2685685760U, 4294963200U, "whois.lacnic.net"),
            // 160.20.96.0 - 160.20.103.255
            Tuple.Create(2685689856U, 4294965248U, "whois.ripe.net"),
            // 160.20.108.0 - 160.20.111.255
            Tuple.Create(2685692928U, 4294966272U, "whois.ripe.net"),
            // 160.20.112.0 - 160.20.115.255
            Tuple.Create(2685693952U, 4294966272U, "whois.afrinic.net"),
            // 160.20.144.0 - 160.20.159.255
            Tuple.Create(2685702144U, 4294963200U, "whois.ripe.net"),
            // 160.20.160.0 - 160.20.207.255
            Tuple.Create(2685706240U, 4294959104U, "whois.lacnic.net"),
            Tuple.Create(2685714432U, 4294963200U, "whois.lacnic.net"),
            // 160.20.208.0 - 160.20.211.255
            Tuple.Create(2685718528U, 4294966272U, "whois.arin.net"),
            // 160.20.213.0 - 160.20.213.255
            Tuple.Create(2685719808U, 4294967040U, "whois.afrinic.net"),
            // 160.20.214.0 - 160.20.215.255
            Tuple.Create(2685720064U, 4294966784U, "whois.ripe.net"),
            // 160.20.217.0 - 160.20.217.255
            Tuple.Create(2685720832U, 4294967040U, "whois.afrinic.net"),
            // 160.20.218.0 - 160.20.219.255
            Tuple.Create(2685721088U, 4294966784U, "whois.lacnic.net"),
            // 160.20.221.0 - 160.20.221.255
            Tuple.Create(2685721856U, 4294967040U, "whois.afrinic.net"),
            // 160.20.222.0 - 160.20.223.255
            Tuple.Create(2685722112U, 4294966784U, "whois.apnic.net"),
            // 160.20.225.0 - 160.20.225.255
            Tuple.Create(2685722880U, 4294967040U, "whois.lacnic.net"),
            // 160.20.226.0 - 160.20.227.255
            Tuple.Create(2685723136U, 4294966784U, "whois.afrinic.net"),
            // 160.20.229.0 - 160.20.229.255
            Tuple.Create(2685723904U, 4294967040U, "whois.ripe.net"),
            // 160.20.230.0 - 160.20.231.255
            Tuple.Create(2685724160U, 4294966784U, "whois.arin.net"),
            // 160.20.232.0 - 160.20.239.255
            Tuple.Create(2685724672U, 4294965248U, "whois.arin.net"),
            // 160.20.242.0 - 160.20.243.255
            Tuple.Create(2685727232U, 4294966784U, "whois.lacnic.net"),
            // 160.20.246.0 - 160.20.247.255
            Tuple.Create(2685728256U, 4294966784U, "whois.lacnic.net"),
            // 160.20.248.0 - 160.20.249.255
            Tuple.Create(2685728768U, 4294966784U, "whois.ripe.net"),
            // 160.20.251.0 - 160.20.251.255
            Tuple.Create(2685729536U, 4294967040U, "whois.arin.net"),
            // 160.20.252.0 - 160.20.255.255
            Tuple.Create(2685729792U, 4294966272U, "whois.afrinic.net"),
            // 160.202.8.0 - 160.202.15.255
            Tuple.Create(2697594880U, 4294965248U, "whois.apnic.net"),
            // 160.202.16.0 - 160.202.31.255
            Tuple.Create(2697596928U, 4294963200U, "whois.ripe.net"),
            // 160.202.32.0 - 160.202.63.255
            Tuple.Create(2697601024U, 4294959104U, "whois.apnic.net"),
            // 160.202.64.0 - 160.202.127.255
            Tuple.Create(2697609216U, 4294950912U, "whois.arin.net"),
            // 160.202.128.0 - 160.202.255.255
            Tuple.Create(2697625600U, 4294934528U, "whois.apnic.net"),
            // 160.238.0.0 - 160.238.0.255
            Tuple.Create(2699952128U, 4294967040U, "whois.apnic.net"),
            // 160.238.11.0 - 160.238.11.255
            Tuple.Create(2699954944U, 4294967040U, "whois.afrinic.net"),
            // 160.238.12.0 - 160.238.19.255
            Tuple.Create(2699955200U, 4294966272U, "whois.apnic.net"),
            Tuple.Create(2699956224U, 4294966272U, "whois.apnic.net"),
            // 160.238.21.0 - 160.238.21.255
            Tuple.Create(2699957504U, 4294967040U, "whois.ripe.net"),
            // 160.238.22.0 - 160.238.23.255
            Tuple.Create(2699957760U, 4294966784U, "whois.arin.net"),
            // 160.238.24.0 - 160.238.29.255
            Tuple.Create(2699958272U, 4294966272U, "whois.lacnic.net"),
            Tuple.Create(2699959296U, 4294966784U, "whois.lacnic.net"),
            // 160.238.31.0 - 160.238.31.255
            Tuple.Create(2699960064U, 4294967040U, "whois.arin.net"),
            // 160.238.33.0 - 160.238.33.255
            Tuple.Create(2699960576U, 4294967040U, "whois.apnic.net"),
            // 160.238.34.0 - 160.238.35.255
            Tuple.Create(2699960832U, 4294966784U, "whois.apnic.net"),
            // 160.238.36.0 - 160.238.39.255
            Tuple.Create(2699961344U, 4294966272U, "whois.ripe.net"),
            // 160.238.41.0 - 160.238.41.255
            Tuple.Create(2699962624U, 4294967040U, "whois.arin.net"),
            // 160.238.42.0 - 160.238.47.255
            Tuple.Create(2699962880U, 4294966784U, "whois.arin.net"),
            Tuple.Create(2699963392U, 4294966272U, "whois.arin.net"),
            // 160.238.48.0 - 160.238.49.255
            Tuple.Create(2699964416U, 4294966784U, "whois.afrinic.net"),
            // 160.238.50.0 - 160.238.50.255
            Tuple.Create(2699964928U, 4294967040U, "whois.afrinic.net"),
            // 160.238.52.0 - 160.238.55.255
            Tuple.Create(2699965440U, 4294966272U, "whois.ripe.net"),
            // 160.238.57.0 - 160.238.57.255
            Tuple.Create(2699966720U, 4294967040U, "whois.afrinic.net"),
            // 160.238.58.0 - 160.238.59.255
            Tuple.Create(2699966976U, 4294966784U, "whois.apnic.net"),
            // 160.238.60.0 - 160.238.61.255
            Tuple.Create(2699967488U, 4294966784U, "whois.ripe.net"),
            // 160.238.63.0 - 160.238.63.255
            Tuple.Create(2699968256U, 4294967040U, "whois.lacnic.net"),
            // 160.238.64.0 - 160.238.95.255
            Tuple.Create(2699968512U, 4294959104U, "whois.apnic.net"),
            // 160.238.96.0 - 160.238.99.255
            Tuple.Create(2699976704U, 4294966272U, "whois.ripe.net"),
            // 160.238.101.0 - 160.238.101.255
            Tuple.Create(2699977984U, 4294967040U, "whois.afrinic.net"),
            // 160.238.102.0 - 160.238.103.255
            Tuple.Create(2699978240U, 4294966784U, "whois.arin.net"),
            // 160.238.104.0 - 160.238.111.255
            Tuple.Create(2699978752U, 4294965248U, "whois.lacnic.net"),
            // 160.238.112.0 - 160.238.127.255
            Tuple.Create(2699980800U, 4294963200U, "whois.ripe.net"),
            // 160.238.128.0 - 160.238.255.255
            Tuple.Create(2699984896U, 4294934528U, "whois.lacnic.net"),
            // 161.123.0.0 - 161.123.255.255
            Tuple.Create(2709192704U, 4294901760U, "whois.afrinic.net"),
            // 162.12.196.0 - 162.12.199.255
            Tuple.Create(2718745600U, 4294966272U, "whois.lacnic.net"),
            // 162.12.200.0 - 162.12.207.255
            Tuple.Create(2718746624U, 4294965248U, "whois.ripe.net"),
            // 162.12.208.0 - 162.12.215.255
            Tuple.Create(2718748672U, 4294965248U, "whois.apnic.net"),
            // 162.12.216.0 - 162.12.219.255
            Tuple.Create(2718750720U, 4294966272U, "whois.arin.net"),
            // 162.12.224.0 - 162.12.235.255
            Tuple.Create(2718752768U, 4294965248U, "whois.arin.net"),
            Tuple.Create(2718754816U, 4294966272U, "whois.arin.net"),
            // 162.12.240.0 - 162.12.247.255
            Tuple.Create(2718756864U, 4294965248U, "whois.apnic.net"),
            // 163.47.4.0 - 163.47.18.255
            Tuple.Create(2737767424U, 4294966272U, "whois.apnic.net"),
            Tuple.Create(2737768448U, 4294965248U, "whois.apnic.net"),
            Tuple.Create(2737770496U, 4294966784U, "whois.apnic.net"),
            Tuple.Create(2737771008U, 4294967040U, "whois.apnic.net"),
            // 163.47.20.0 - 163.47.21.255
            Tuple.Create(2737771520U, 4294966784U, "whois.apnic.net"),
            // 163.47.32.0 - 163.47.45.255
            Tuple.Create(2737774592U, 4294965248U, "whois.apnic.net"),
            Tuple.Create(2737776640U, 4294966272U, "whois.apnic.net"),
            Tuple.Create(2737777664U, 4294966784U, "whois.apnic.net"),
            // 163.47.47.0 - 163.47.255.255
            Tuple.Create(2737778432U, 4294967040U, "whois.apnic.net"),
            Tuple.Create(2737778688U, 4294963200U, "whois.apnic.net"),
            Tuple.Create(2737782784U, 4294950912U, "whois.apnic.net"),
            Tuple.Create(2737799168U, 4294934528U, "whois.apnic.net"),
            // 163.53.0.0 - 163.53.255.255
            Tuple.Create(2738159616U, 4294901760U, "whois.apnic.net"),
            // 164.160.0.0 - 164.160.255.255
            Tuple.Create(2761949184U, 4294901760U, "whois.afrinic.net"),
            // 164.163.0.0 - 164.163.255.255
            Tuple.Create(2762145792U, 4294901760U, "whois.lacnic.net"),
            // 192.12.109.0 - 192.12.109.255
            Tuple.Create(3222039808U, 4294967040U, "whois.apnic.net"),
            // 192.12.110.0 - 192.12.111.255
            Tuple.Create(3222040064U, 4294966784U, "whois.afrinic.net"),
            // 192.12.112.0 - 192.12.115.255
            Tuple.Create(3222040576U, 4294966272U, "whois.lacnic.net"),
            // 192.12.116.0 - 192.12.117.255
            Tuple.Create(3222041600U, 4294966784U, "whois.afrinic.net"),
            // 192.12.118.0 - 192.12.118.255
            Tuple.Create(3222042112U, 4294967040U, "whois.lacnic.net"),
            // 192.26.110.0 - 192.26.110.255
            Tuple.Create(3222957568U, 4294967040U, "whois.apnic.net"),
            // 192.42.65.0 - 192.42.65.255
            Tuple.Create(3223994624U, 4294967040U, "whois.ripe.net"),
            // 192.47.36.0 - 192.47.36.255
            Tuple.Create(3224314880U, 4294967040U, "whois.afrinic.net"),
            // 192.51.188.0 - 192.51.188.255
            Tuple.Create(3224615936U, 4294967040U, "whois.apnic.net"),
            // 192.51.240.0 - 192.51.240.255
            Tuple.Create(3224629248U, 4294967040U, "whois.afrinic.net"),
            // 192.54.244.0 - 192.54.244.255
            Tuple.Create(3224826880U, 4294967040U, "whois.ripe.net"),
            // 192.67.23.0 - 192.67.23.255
            Tuple.Create(3225622272U, 4294967040U, "whois.lacnic.net"),
            // 192.68.185.0 - 192.68.185.255
            Tuple.Create(3225729280U, 4294967040U, "whois.lacnic.net"),
            // 192.70.192.0 - 192.70.199.255
            Tuple.Create(3225862144U, 4294965248U, "whois.ripe.net"),
            // 192.70.200.0 - 192.70.201.255
            Tuple.Create(3225864192U, 4294966784U, "whois.afrinic.net"),
            // 192.75.4.0 - 192.75.4.255
            Tuple.Create(3226141696U, 4294967040U, "whois.lacnic.net"),
            // 192.75.137.0 - 192.75.137.255
            Tuple.Create(3226175744U, 4294967040U, "whois.apnic.net"),
            // 192.75.236.0 - 192.75.236.255
            Tuple.Create(3226201088U, 4294967040U, "whois.afrinic.net"),
            // 192.83.207.0 - 192.83.207.255
            Tuple.Create(3226717952U, 4294967040U, "whois.lacnic.net"),
            // 192.83.208.0 - 192.83.215.255
            Tuple.Create(3226718208U, 4294965248U, "whois.afrinic.net"),
            // 192.83.216.0 - 192.83.216.255
            Tuple.Create(3226720256U, 4294967040U, "whois.ripe.net"),
            // 192.91.200.0 - 192.91.200.255
            Tuple.Create(3227240448U, 4294967040U, "whois.afrinic.net"),
            // 192.91.254.0 - 192.91.254.255
            Tuple.Create(3227254272U, 4294967040U, "whois.lacnic.net"),
            // 192.92.154.0 - 192.92.154.255
            Tuple.Create(3227294208U, 4294967040U, "whois.lacnic.net"),
            // 192.94.77.0 - 192.94.77.255
            Tuple.Create(3227405568U, 4294967040U, "whois.arin.net"),
            // 192.94.78.0 - 192.94.78.255
            Tuple.Create(3227405824U, 4294967040U, "whois.ripe.net"),
            // 192.107.1.0 - 192.107.1.255
            Tuple.Create(3228238080U, 4294967040U, "whois.arin.net"),
            // 192.133.103.0 - 192.133.103.255
            Tuple.Create(3229968128U, 4294967040U, "whois.arin.net"),
            // 192.135.90.0 - 192.135.91.255
            Tuple.Create(3230095872U, 4294966784U, "whois.apnic.net"),
            // 192.135.95.0 - 192.135.95.255
            Tuple.Create(3230097152U, 4294967040U, "whois.lacnic.net"),
            // 192.135.99.0 - 192.135.99.255
            Tuple.Create(3230098176U, 4294967040U, "whois.apnic.net"),
            // 192.135.100.0 - 192.135.100.255
            Tuple.Create(3230098432U, 4294967040U, "whois.ripe.net"),
            // 192.135.185.0 - 192.135.185.255
            Tuple.Create(3230120192U, 4294967040U, "whois.lacnic.net"),
            // 192.140.1.0 - 192.140.1.255
            Tuple.Create(3230400768U, 4294967040U, "whois.lacnic.net"),
            // 192.140.2.0 - 192.140.3.255
            Tuple.Create(3230401024U, 4294966784U, "whois.ripe.net"),
            // 192.140.4.0 - 192.140.7.255
            Tuple.Create(3230401536U, 4294966272U, "whois.arin.net"),
            // 192.140.8.0 - 192.140.15.255
            Tuple.Create(3230402560U, 4294965248U, "whois.lacnic.net"),
            // 192.140.16.0 - 192.140.127.255
            Tuple.Create(3230404608U, 4294963200U, "whois.lacnic.net"),
            Tuple.Create(3230408704U, 4294959104U, "whois.lacnic.net"),
            Tuple.Create(3230416896U, 4294950912U, "whois.lacnic.net"),
            // 192.140.128.0 - 192.140.255.255
            Tuple.Create(3230433280U, 4294934528U, "whois.apnic.net"),
            // 192.141.0.0 - 192.141.255.255
            Tuple.Create(3230466048U, 4294901760U, "whois.lacnic.net"),
            // 192.142.0.0 - 192.143.255.255
            Tuple.Create(3230531584U, 4294836224U, "whois.afrinic.net"),
            // 192.144.0.0 - 192.144.63.255
            Tuple.Create(3230662656U, 4294950912U, "whois.ripe.net"),
            // 192.144.64.0 - 192.144.71.255
            Tuple.Create(3230679040U, 4294965248U, "whois.lacnic.net"),
            // 192.144.72.0 - 192.144.73.255
            Tuple.Create(3230681088U, 4294966784U, "whois.lacnic.net"),
            // 192.144.75.0 - 192.144.75.255
            Tuple.Create(3230681856U, 4294967040U, "whois.ripe.net"),
            // 192.144.78.0 - 192.144.79.255
            Tuple.Create(3230682624U, 4294966784U, "whois.apnic.net"),
            // 192.144.80.0 - 192.144.95.255
            Tuple.Create(3230683136U, 4294963200U, "whois.apnic.net"),
            // 192.144.96.0 - 192.144.127.255
            Tuple.Create(3230687232U, 4294959104U, "whois.lacnic.net"),
            // 192.144.128.0 - 192.144.255.255
            Tuple.Create(3230695424U, 4294934528U, "whois.arin.net"),
            // 192.145.0.0 - 192.145.127.255
            Tuple.Create(3230728192U, 4294934528U, "whois.ripe.net"),
            // 192.145.128.0 - 192.145.191.255
            Tuple.Create(3230760960U, 4294950912U, "whois.afrinic.net"),
            // 192.145.192.0 - 192.145.223.255
            Tuple.Create(3230777344U, 4294959104U, "whois.lacnic.net"),
            // 192.145.224.0 - 192.145.227.255
            Tuple.Create(3230785536U, 4294966272U, "whois.ripe.net"),
            // 192.145.228.0 - 192.145.229.255
            Tuple.Create(3230786560U, 4294966784U, "whois.apnic.net"),
            // 192.145.230.0 - 192.145.230.255
            Tuple.Create(3230787072U, 4294967040U, "whois.afrinic.net"),
            // 192.147.11.0 - 192.147.11.255
            Tuple.Create(3230862080U, 4294967040U, "whois.arin.net"),
            // 192.153.12.0 - 192.153.12.255
            Tuple.Create(3231255552U, 4294967040U, "whois.lacnic.net"),
            // 192.156.144.0 - 192.156.144.255
            Tuple.Create(3231485952U, 4294967040U, "whois.apnic.net"),
            // 192.156.202.0 - 192.156.202.255
            Tuple.Create(3231500800U, 4294967040U, "whois.arin.net"),
            // 192.156.220.0 - 192.156.220.255
            Tuple.Create(3231505408U, 4294967040U, "whois.apnic.net"),
            // 192.172.232.0 - 192.172.232.255
            Tuple.Create(3232557056U, 4294967040U, "whois.ripe.net"),
            // 192.172.244.0 - 192.172.244.255
            Tuple.Create(3232560128U, 4294967040U, "whois.arin.net"),
            // 192.188.81.0 - 192.188.81.255
            Tuple.Create(3233566976U, 4294967040U, "whois.arin.net"),
            // 192.188.82.0 - 192.188.83.255
            Tuple.Create(3233567232U, 4294966784U, "whois.apnic.net"),
            // 192.188.248.0 - 192.188.248.255
            Tuple.Create(3233609728U, 4294967040U, "whois.ripe.net"),
            // 192.197.113.0 - 192.197.113.255
            Tuple.Create(3234164992U, 4294967040U, "whois.apnic.net"),
            // 192.231.238.0 - 192.231.238.255
            Tuple.Create(3236425216U, 4294967040U, "whois.arin.net"),
            // 192.251.230.0 - 192.251.230.255
            Tuple.Create(3237733888U, 4294967040U, "whois.ripe.net"),
            // 198.17.79.0 - 198.17.79.255
            Tuple.Create(3323023104U, 4294967040U, "whois.arin.net"),
            // 198.97.38.0 - 198.97.38.255
            Tuple.Create(3328255488U, 4294967040U, "whois.lacnic.net"),
            // 199.21.172.0 - 199.21.175.255
            Tuple.Create(3340086272U, 4294966272U, "whois.apnic.net"),
            // 199.212.57.0 - 199.212.57.255
            Tuple.Create(3352574208U, 4294967040U, "whois.apnic.net"),
            // 204.8.204.0 - 204.8.207.255
            Tuple.Create(3423128576U, 4294966272U, "whois.afrinic.net"),
            // 204.11.0.0 - 204.11.3.255
            Tuple.Create(3423272960U, 4294966272U, "whois.ripe.net"),
            // 204.48.32.0 - 204.48.33.255
            Tuple.Create(3425705984U, 4294966784U, "whois.ripe.net"),
            // 204.52.191.0 - 204.52.191.255
            Tuple.Create(3426008832U, 4294967040U, "whois.apnic.net"),
            // 204.225.42.0 - 204.225.43.255
            Tuple.Create(3437308416U, 4294966784U, "whois.lacnic.net"),
            // 205.211.83.0 - 205.211.83.255
            Tuple.Create(3453178624U, 4294967040U, "whois.ripe.net"),
            // 207.115.112.0 - 207.115.127.255
            Tuple.Create(3480449024U, 4294963200U, "whois.arin.net"),
            // 208.73.240.0 - 208.73.243.255
            Tuple.Create(3494506496U, 4294966272U, "whois.arin.net"),
            // 208.85.156.0 - 208.85.159.255
            Tuple.Create(3495271424U, 4294966272U, "whois.afrinic.net"),
            // 209.107.128.0 - 209.107.191.255
            Tuple.Create(3513483264U, 4294950912U, "whois.arin.net"),
            // 216.98.208.0 - 216.98.223.255
            Tuple.Create(3630354432U, 4294963200U, "whois.lacnic.net"),
            // 216.250.96.0 - 216.250.111.255
            Tuple.Create(3640287232U, 4294963200U, "whois.apnic.net"),
        //
        // http://www.iana.org/assignments/ipv4-address-space
        //
            // 0.0.0.0/8
            Tuple.Create(0U, 4278190080U, "\x05"),
            // 1.0.0.0/8
            Tuple.Create(16777216U, 4278190080U, "whois.apnic.net"),
            // 2.0.0.0/8
            Tuple.Create(33554432U, 4278190080U, "whois.ripe.net"),
            // 5.0.0.0/8
            Tuple.Create(83886080U, 4278190080U, "whois.ripe.net"),
            // 14.0.0.0/8
            Tuple.Create(234881024U, 4278190080U, "whois.apnic.net"),
            // 24.132.0.0/14
            Tuple.Create(411303936U, 4294705152U, "whois.ripe.net"),
            // 27.0.0.0/8
            Tuple.Create(452984832U, 4278190080U, "whois.apnic.net"),
            // 31.0.0.0/8
            Tuple.Create(520093696U, 4278190080U, "whois.ripe.net"),
            // 36.0.0.0/8
            Tuple.Create(603979776U, 4278190080U, "whois.apnic.net"),
            // 37.0.0.0/8
            Tuple.Create(620756992U, 4278190080U, "whois.ripe.net"),
            // 39.0.0.0/8
            Tuple.Create(654311424U, 4278190080U, "whois.apnic.net"),
            // 41.0.0.0/8
            Tuple.Create(687865856U, 4278190080U, "whois.afrinic.net"),
            // 42.0.0.0/8
            Tuple.Create(704643072U, 4278190080U, "whois.apnic.net"),
            // 43.224.0.0/11
            Tuple.Create(736100352U, 4292870144U, "whois.apnic.net"),
            // 43.0.0.0/8
            Tuple.Create(721420288U, 4278190080U, "whois.nic.ad.jp"),
            // 46.0.0.0/8
            Tuple.Create(771751936U, 4278190080U, "whois.ripe.net"),
            // 49.0.0.0/8
            Tuple.Create(822083584U, 4278190080U, "whois.apnic.net"),
            // 51.0.0.0/8
            Tuple.Create(855638016U, 4278190080U, "whois.ripe.net"),
            // 59.0.0.0/11
            Tuple.Create(989855744U, 4292870144U, "whois.nic.or.kr"),
            // 58.0.0.0/7
            Tuple.Create(973078528U, 4261412864U, "whois.apnic.net"),
            // 61.72.0.0/13
            Tuple.Create(1028128768U, 4294443008U, "whois.nic.or.kr"),
            // 61.80.0.0/14
            Tuple.Create(1028653056U, 4294705152U, "whois.nic.or.kr"),
            // 61.84.0.0/15
            Tuple.Create(1028915200U, 4294836224U, "whois.nic.or.kr"),
            // 61.112.0.0/12
            Tuple.Create(1030750208U, 4293918720U, "whois.nic.ad.jp"),
            // 61.192.0.0/12
            Tuple.Create(1035993088U, 4293918720U, "whois.nic.ad.jp"),
            // 61.208.0.0/13
            Tuple.Create(1037041664U, 4294443008U, "whois.nic.ad.jp"),
            // 60.0.0.0/7
            Tuple.Create(1006632960U, 4261412864U, "whois.apnic.net"),
            // 62.0.0.0/8
            Tuple.Create(1040187392U, 4278190080U, "whois.ripe.net"),
            // 77.0.0.0/8
            Tuple.Create(1291845632U, 4278190080U, "whois.ripe.net"),
            // 78.0.0.0/7
            Tuple.Create(1308622848U, 4261412864U, "whois.ripe.net"),
            // 80.0.0.0/4
            Tuple.Create(1342177280U, 4026531840U, "whois.ripe.net"),
            // 101.0.0.0/8
            Tuple.Create(1694498816U, 4278190080U, "whois.apnic.net"),
            // 102.0.0.0/8
            Tuple.Create(1711276032U, 4278190080U, "whois.afrinic.net"),
            // 103.0.0.0/8
            Tuple.Create(1728053248U, 4278190080U, "whois.apnic.net"),
            // 105.0.0.0/8
            Tuple.Create(1761607680U, 4278190080U, "whois.afrinic.net"),
            // 106.0.0.0/8
            Tuple.Create(1778384896U, 4278190080U, "whois.apnic.net"),
            // 109.0.0.0/8
            Tuple.Create(1828716544U, 4278190080U, "whois.ripe.net"),
            // 110.0.0.0/7
            Tuple.Create(1845493760U, 4261412864U, "whois.apnic.net"),
            // 112.160.0.0/11
            Tuple.Create(1889533952U, 4292870144U, "whois.nic.or.kr"),
            // 115.0.0.0/12
            Tuple.Create(1929379840U, 4293918720U, "whois.nic.or.kr"),
            // 115.16.0.0/13
            Tuple.Create(1930428416U, 4294443008U, "whois.nic.or.kr"),
            // 118.32.0.0/11
            Tuple.Create(1981808640U, 4292870144U, "whois.nic.or.kr"),
            // 119.192.0.0/11
            Tuple.Create(2009071616U, 4292870144U, "whois.nic.or.kr"),
            // 112.0.0.0/5
            Tuple.Create(1879048192U, 4160749568U, "whois.apnic.net"),
            // 121.128.0.0/10
            Tuple.Create(2038431744U, 4290772992U, "whois.nic.or.kr"),
            // 125.128.0.0/11
            Tuple.Create(2105540608U, 4292870144U, "whois.nic.or.kr"),
            // 120.0.0.0/6
            Tuple.Create(2013265920U, 4227858432U, "whois.apnic.net"),
            // 124.0.0.0/7
            Tuple.Create(2080374784U, 4261412864U, "whois.apnic.net"),
            // 126.0.0.0/8
            Tuple.Create(2113929216U, 4278190080U, "whois.apnic.net"),
            // 0.0.0.0/1
            Tuple.Create(0U, 2147483648U, "whois.arin.net"),
            // 133.0.0.0/8
            Tuple.Create(2231369728U, 4278190080U, "whois.nic.ad.jp"),
            // 139.20.0.0/14
            Tuple.Create(2333343744U, 4294705152U, "whois.ripe.net"),
            // 139.24.0.0/14
            Tuple.Create(2333605888U, 4294705152U, "whois.ripe.net"),
            // 139.28.0.0/15
            Tuple.Create(2333868032U, 4294836224U, "whois.ripe.net"),
            // 141.0.0.0/10
            Tuple.Create(2365587456U, 4290772992U, "whois.ripe.net"),
            // 141.86.0.0/16
            Tuple.Create(2371223552U, 4294901760U, "whois.arin.net"),
            // 141.64.0.0/11
            Tuple.Create(2369781760U, 4292870144U, "whois.ripe.net"),
            // 141.96.0.0/14
            Tuple.Create(2371878912U, 4294705152U, "whois.ripe.net"),
            // 141.100.0.0/16
            Tuple.Create(2372141056U, 4294901760U, "whois.ripe.net"),
            // 145.0.0.0/8
            Tuple.Create(2432696320U, 4278190080U, "whois.ripe.net"),
            // 146.48.0.0/16
            Tuple.Create(2452619264U, 4294901760U, "whois.ripe.net"),
            // 149.202.0.0/15
            Tuple.Create(2513043456U, 4294836224U, "whois.ripe.net"),
            // 149.204.0.0/16
            Tuple.Create(2513174528U, 4294901760U, "whois.ripe.net"),
            // 149.206.0.0/15
            Tuple.Create(2513305600U, 4294836224U, "whois.ripe.net"),
            // 149.208.0.0/12
            Tuple.Create(2513436672U, 4293918720U, "whois.ripe.net"),
            // 149.224.0.0/12
            Tuple.Create(2514485248U, 4293918720U, "whois.ripe.net"),
            // 149.240.0.0/13
            Tuple.Create(2515533824U, 4294443008U, "whois.ripe.net"),
            // 149.248.0.0/14
            Tuple.Create(2516058112U, 4294705152U, "whois.ripe.net"),
            // 150.183.0.0/16
            Tuple.Create(2528575488U, 4294901760U, "whois.nic.or.kr"),
            // 150.254.0.0/16
            Tuple.Create(2533228544U, 4294901760U, "whois.ripe.net"),
            // 150.0.0.0/8
            Tuple.Create(2516582400U, 4278190080U, "whois.apnic.net"),
            // 151.0.0.0/10
            Tuple.Create(2533359616U, 4290772992U, "whois.ripe.net"),
            // 151.64.0.0/11
            Tuple.Create(2537553920U, 4292870144U, "whois.ripe.net"),
            // 151.96.0.0/14
            Tuple.Create(2539651072U, 4294705152U, "whois.ripe.net"),
            // 151.100.0.0/16
            Tuple.Create(2539913216U, 4294901760U, "whois.ripe.net"),
            // 153.128.0.0/9
            Tuple.Create(2575302656U, 4286578688U, "whois.nic.ad.jp"),
            // 153.0.0.0/8
            Tuple.Create(2566914048U, 4278190080U, "whois.apnic.net"),
            // 154.0.0.0/8
            Tuple.Create(2583691264U, 4278190080U, "whois.afrinic.net"),
            // 155.232.0.0/13
            Tuple.Create(2615672832U, 4294443008U, "whois.afrinic.net"),
            // 155.240.0.0/16
            Tuple.Create(2616197120U, 4294901760U, "whois.afrinic.net"),
            // 160.216.0.0/14
            Tuple.Create(2698510336U, 4294705152U, "whois.ripe.net"),
            // 160.220.0.0/16
            Tuple.Create(2698772480U, 4294901760U, "whois.ripe.net"),
            // 160.44.0.0/14
            Tuple.Create(2687238144U, 4294705152U, "whois.ripe.net"),
            // 160.48.0.0/12
            Tuple.Create(2687500288U, 4293918720U, "whois.ripe.net"),
            // 160.115.0.0/16
            Tuple.Create(2691891200U, 4294901760U, "whois.afrinic.net"),
            // 160.116.0.0/14
            Tuple.Create(2691956736U, 4294705152U, "whois.afrinic.net"),
            // 160.120.0.0/14
            Tuple.Create(2692218880U, 4294705152U, "whois.afrinic.net"),
            // 160.124.0.0/16
            Tuple.Create(2692481024U, 4294901760U, "whois.afrinic.net"),
            // 163.156.0.0/14
            Tuple.Create(2744909824U, 4294705152U, "whois.ripe.net"),
            // 163.160.0.0/12
            Tuple.Create(2745171968U, 4293918720U, "whois.ripe.net"),
            // 163.195.0.0/16
            Tuple.Create(2747465728U, 4294901760U, "whois.afrinic.net"),
            // 163.196.0.0/14
            Tuple.Create(2747531264U, 4294705152U, "whois.afrinic.net"),
            // 163.200.0.0/14
            Tuple.Create(2747793408U, 4294705152U, "whois.afrinic.net"),
            // 163.0.0.0/8
            Tuple.Create(2734686208U, 4278190080U, "whois.apnic.net"),
            // 164.0.0.0/11
            Tuple.Create(2751463424U, 4292870144U, "whois.ripe.net"),
            // 164.32.0.0/13
            Tuple.Create(2753560576U, 4294443008U, "whois.ripe.net"),
            // 164.40.0.0/16
            Tuple.Create(2754084864U, 4294901760U, "whois.ripe.net"),
            // 164.128.0.0/12
            Tuple.Create(2759852032U, 4293918720U, "whois.ripe.net"),
            // 164.146.0.0/15
            Tuple.Create(2761031680U, 4294836224U, "whois.afrinic.net"),
            // 164.148.0.0/14
            Tuple.Create(2761162752U, 4294705152U, "whois.afrinic.net"),
            // 165.143.0.0/16
            Tuple.Create(2777612288U, 4294901760U, "whois.afrinic.net"),
            // 165.144.0.0/14
            Tuple.Create(2777677824U, 4294705152U, "whois.afrinic.net"),
            // 165.148.0.0/15
            Tuple.Create(2777939968U, 4294836224U, "whois.afrinic.net"),
            // 169.208.0.0/12
            Tuple.Create(2848980992U, 4293918720U, "whois.apnic.net"),
            // 171.16.0.0/12
            Tuple.Create(2869952512U, 4293918720U, "whois.ripe.net"),
            // 171.32.0.0/15
            Tuple.Create(2871001088U, 4294836224U, "whois.ripe.net"),
            // 171.0.0.0/8
            Tuple.Create(2868903936U, 4278190080U, "whois.apnic.net"),
            // 175.192.0.0/10
            Tuple.Create(2948595712U, 4290772992U, "whois.nic.or.kr"),
            // 175.0.0.0/8
            Tuple.Create(2936012800U, 4278190080U, "whois.apnic.net"),
            // 176.0.0.0/8
            Tuple.Create(2952790016U, 4278190080U, "whois.ripe.net"),
            // 177.0.0.0/8
            Tuple.Create(2969567232U, 4278190080U, "whois.lacnic.net"),
            // 178.0.0.0/8
            Tuple.Create(2986344448U, 4278190080U, "whois.ripe.net"),
            // 179.0.0.0/8
            Tuple.Create(3003121664U, 4278190080U, "whois.lacnic.net"),
            // 180.0.0.0/8
            Tuple.Create(3019898880U, 4278190080U, "whois.apnic.net"),
            // 181.0.0.0/8
            Tuple.Create(3036676096U, 4278190080U, "whois.lacnic.net"),
            // 183.96.0.0/11
            Tuple.Create(3076521984U, 4292870144U, "whois.nic.or.kr"),
            // 182.0.0.0/7
            Tuple.Create(3053453312U, 4261412864U, "whois.apnic.net"),
            // 185.0.0.0/8
            Tuple.Create(3103784960U, 4278190080U, "whois.ripe.net"),
            // 186.0.0.0/7
            Tuple.Create(3120562176U, 4261412864U, "whois.lacnic.net"),
            // 188.0.0.0/8
            Tuple.Create(3154116608U, 4278190080U, "whois.ripe.net"),
            // 189.0.0.0/8
            Tuple.Create(3170893824U, 4278190080U, "whois.lacnic.net"),
            // 190.0.0.0/7
            Tuple.Create(3187671040U, 4261412864U, "whois.lacnic.net"),
            // 128.0.0.0/2
            Tuple.Create(2147483648U, 3221225472U, "whois.arin.net"),
            // 192.71.0.0/16
            Tuple.Create(3225878528U, 4294901760U, "whois.ripe.net"),
            // 192.72.253.0/24
            Tuple.Create(3226008832U, 4294967040U, "whois.arin.net"),
            // 192.72.254.0/24
            Tuple.Create(3226009088U, 4294967040U, "whois.arin.net"),
            // 192.72.0.0/16
            Tuple.Create(3225944064U, 4294901760U, "whois.apnic.net"),
            // 192.106.0.0/16
            Tuple.Create(3228172288U, 4294901760U, "whois.ripe.net"),
            // 192.114.0.0/15
            Tuple.Create(3228696576U, 4294836224U, "whois.ripe.net"),
            // 192.116.0.0/15
            Tuple.Create(3228827648U, 4294836224U, "whois.ripe.net"),
            // 192.118.0.0/16
            Tuple.Create(3228958720U, 4294901760U, "whois.ripe.net"),
            // 192.162.0.0/16
            Tuple.Create(3231842304U, 4294901760U, "whois.ripe.net"),
            // 192.164.0.0/14
            Tuple.Create(3231973376U, 4294705152U, "whois.ripe.net"),
            // 192.0.0.0/8
            Tuple.Create(3221225472U, 4278190080U, "whois.arin.net"),
            // 193.0.0.0/8
            Tuple.Create(3238002688U, 4278190080U, "whois.ripe.net"),
            // 194.0.0.0/7
            Tuple.Create(3254779904U, 4261412864U, "whois.ripe.net"),
            // 196.0.0.0/7
            Tuple.Create(3288334336U, 4261412864U, "whois.afrinic.net"),
            // 198.0.0.0/7
            Tuple.Create(3321888768U, 4261412864U, "whois.arin.net"),
            // 200.0.0.0/7
            Tuple.Create(3355443200U, 4261412864U, "whois.lacnic.net"),
            // 202.11.0.0/16
            Tuple.Create(3389718528U, 4294901760U, "whois.nic.ad.jp"),
            // 202.13.0.0/16
            Tuple.Create(3389849600U, 4294901760U, "whois.nic.ad.jp"),
            // 202.15.0.0/16
            Tuple.Create(3389980672U, 4294901760U, "whois.nic.ad.jp"),
            // 202.16.0.0/14
            Tuple.Create(3390046208U, 4294705152U, "whois.nic.ad.jp"),
            // 202.20.128.0/17
            Tuple.Create(3390341120U, 4294934528U, "whois.nic.or.kr"),
            // 202.23.0.0/16
            Tuple.Create(3390504960U, 4294901760U, "whois.nic.ad.jp"),
            // 202.24.0.0/15
            Tuple.Create(3390570496U, 4294836224U, "whois.nic.ad.jp"),
            // 202.26.0.0/16
            Tuple.Create(3390701568U, 4294901760U, "whois.nic.ad.jp"),
            // 202.30.0.0/15
            Tuple.Create(3390963712U, 4294836224U, "whois.nic.or.kr"),
            // 202.32.0.0/14
            Tuple.Create(3391094784U, 4294705152U, "whois.nic.ad.jp"),
            // 202.48.0.0/16
            Tuple.Create(3392143360U, 4294901760U, "whois.nic.ad.jp"),
            // 202.39.128.0/17
            Tuple.Create(3391586304U, 4294934528U, "whois.twnic.net"),
            // 202.208.0.0/12
            Tuple.Create(3402629120U, 4293918720U, "whois.nic.ad.jp"),
            // 202.224.0.0/11
            Tuple.Create(3403677696U, 4292870144U, "whois.nic.ad.jp"),
            // 203.0.0.0/10
            Tuple.Create(3405774848U, 4290772992U, "whois.apnic.net"),
            // 203.66.0.0/16
            Tuple.Create(3410100224U, 4294901760U, "whois.twnic.net"),
            // 203.69.0.0/16
            Tuple.Create(3410296832U, 4294901760U, "whois.twnic.net"),
            // 203.74.0.0/15
            Tuple.Create(3410624512U, 4294836224U, "whois.twnic.net"),
            // 203.136.0.0/14
            Tuple.Create(3414687744U, 4294705152U, "whois.nic.ad.jp"),
            // 203.140.0.0/15
            Tuple.Create(3414949888U, 4294836224U, "whois.nic.ad.jp"),
            // 203.178.0.0/15
            Tuple.Create(3417440256U, 4294836224U, "whois.nic.ad.jp"),
            // 203.180.0.0/14
            Tuple.Create(3417571328U, 4294705152U, "whois.nic.ad.jp"),
            // 203.224.0.0/11
            Tuple.Create(3420454912U, 4292870144U, "whois.nic.or.kr"),
            // 202.0.0.0/7
            Tuple.Create(3388997632U, 4261412864U, "whois.apnic.net"),
            // 204.0.0.0/14
            Tuple.Create(3422552064U, 4294705152U, "rwhois.gin.ntt.net"),
            // 204.0.0.0/6
            Tuple.Create(3422552064U, 4227858432U, "whois.arin.net"),
            // 208.0.0.0/7
            Tuple.Create(3489660928U, 4261412864U, "whois.arin.net"),
            // 209.94.192.0/19
            Tuple.Create(3512647680U, 4294959104U, "whois.lacnic.net"),
            // 210.59.128.0/17
            Tuple.Create(3527114752U, 4294934528U, "whois.twnic.net"),
            // 210.61.0.0/16
            Tuple.Create(3527213056U, 4294901760U, "whois.twnic.net"),
            // 210.62.252.0/22
            Tuple.Create(3527343104U, 4294966272U, "whois.twnic.net"),
            // 210.65.0.0/16
            Tuple.Create(3527475200U, 4294901760U, "whois.twnic.net"),
            // 210.71.128.0/17
            Tuple.Create(3527901184U, 4294934528U, "whois.twnic.net"),
            // 210.90.0.0/15
            Tuple.Create(3529113600U, 4294836224U, "whois.nic.or.kr"),
            // 210.92.0.0/14
            Tuple.Create(3529244672U, 4294705152U, "whois.nic.or.kr"),
            // 210.96.0.0/11
            Tuple.Create(3529506816U, 4292870144U, "whois.nic.or.kr"),
            // 210.128.0.0/11
            Tuple.Create(3531603968U, 4292870144U, "whois.nic.ad.jp"),
            // 210.160.0.0/12
            Tuple.Create(3533701120U, 4293918720U, "whois.nic.ad.jp"),
            // 210.178.0.0/15
            Tuple.Create(3534880768U, 4294836224U, "whois.nic.or.kr"),
            // 210.180.0.0/14
            Tuple.Create(3535011840U, 4294705152U, "whois.nic.or.kr"),
            // 210.188.0.0/14
            Tuple.Create(3535536128U, 4294705152U, "whois.nic.ad.jp"),
            // 210.196.0.0/14
            Tuple.Create(3536060416U, 4294705152U, "whois.nic.ad.jp"),
            // 210.204.0.0/14
            Tuple.Create(3536584704U, 4294705152U, "whois.nic.or.kr"),
            // 210.216.0.0/13
            Tuple.Create(3537371136U, 4294443008U, "whois.nic.or.kr"),
            // 210.224.0.0/12
            Tuple.Create(3537895424U, 4293918720U, "whois.nic.ad.jp"),
            // 210.240.0.0/16
            Tuple.Create(3538944000U, 4294901760U, "whois.twnic.net"),
            // 210.241.0.0/18
            Tuple.Create(3539009536U, 4294950912U, "whois.twnic.net"),
            // 210.241.224.0/19
            Tuple.Create(3539066880U, 4294959104U, "whois.twnic.net"),
            // 210.242.0.0/15
            Tuple.Create(3539075072U, 4294836224U, "whois.twnic.net"),
            // 210.248.0.0/13
            Tuple.Create(3539468288U, 4294443008U, "whois.nic.ad.jp"),
            // 211.0.0.0/12
            Tuple.Create(3539992576U, 4293918720U, "whois.nic.ad.jp"),
            // 211.16.0.0/14
            Tuple.Create(3541041152U, 4294705152U, "whois.nic.ad.jp"),
            // 211.20.0.0/15
            Tuple.Create(3541303296U, 4294836224U, "whois.twnic.net"),
            // 211.22.0.0/16
            Tuple.Create(3541434368U, 4294901760U, "whois.twnic.net"),
            // 211.32.0.0/11
            Tuple.Create(3542089728U, 4292870144U, "whois.nic.or.kr"),
            // 211.75.0.0/16
            Tuple.Create(3544907776U, 4294901760U, "whois.twnic.net"),
            // 211.72.0.0/16
            Tuple.Create(3544711168U, 4294901760U, "whois.twnic.net"),
            // 211.104.0.0/13
            Tuple.Create(3546808320U, 4294443008U, "whois.nic.or.kr"),
            // 211.112.0.0/13
            Tuple.Create(3547332608U, 4294443008U, "whois.nic.or.kr"),
            // 211.120.0.0/13
            Tuple.Create(3547856896U, 4294443008U, "whois.nic.ad.jp"),
            // 211.128.0.0/13
            Tuple.Create(3548381184U, 4294443008U, "whois.nic.ad.jp"),
            // 211.168.0.0/13
            Tuple.Create(3551002624U, 4294443008U, "whois.nic.or.kr"),
            // 211.176.0.0/12
            Tuple.Create(3551526912U, 4293918720U, "whois.nic.or.kr"),
            // 211.192.0.0/10
            Tuple.Create(3552575488U, 4290772992U, "whois.nic.or.kr"),
            // 210.0.0.0/7
            Tuple.Create(3523215360U, 4261412864U, "whois.apnic.net"),
            // 213.154.32.0/19
            Tuple.Create(3583647744U, 4294959104U, "whois.afrinic.net"),
            // 213.154.64.0/19
            Tuple.Create(3583655936U, 4294959104U, "whois.afrinic.net"),
            // 212.0.0.0/7
            Tuple.Create(3556769792U, 4261412864U, "whois.ripe.net"),
            // 214.0.0.0/7
            Tuple.Create(3590324224U, 4261412864U, "whois.arin.net"),
            // 216.0.0.0/8
            Tuple.Create(3623878656U, 4278190080U, "whois.arin.net"),
            // 217.0.0.0/8
            Tuple.Create(3640655872U, 4278190080U, "whois.ripe.net"),
            // 218.36.0.0/14
            Tuple.Create(3659792384U, 4294705152U, "whois.nic.or.kr"),
            // 218.40.0.0/13
            Tuple.Create(3660054528U, 4294443008U, "whois.nic.ad.jp"),
            // 218.48.0.0/13
            Tuple.Create(3660578816U, 4294443008U, "whois.nic.or.kr"),
            // 219.96.0.0/11
            Tuple.Create(3680501760U, 4292870144U, "whois.nic.ad.jp"),
            // 218.144.0.0/12
            Tuple.Create(3666870272U, 4293918720U, "whois.nic.or.kr"),
            // 218.160.0.0/12
            Tuple.Create(3667918848U, 4293918720U, "whois.twnic.net"),
            // 218.216.0.0/13
            Tuple.Create(3671588864U, 4294443008U, "whois.nic.ad.jp"),
            // 218.224.0.0/13
            Tuple.Create(3672113152U, 4294443008U, "whois.nic.ad.jp"),
            // 218.232.0.0/13
            Tuple.Create(3672637440U, 4294443008U, "whois.nic.or.kr"),
            // 219.240.0.0/15
            Tuple.Create(3689938944U, 4294836224U, "whois.nic.or.kr"),
            // 219.248.0.0/13
            Tuple.Create(3690463232U, 4294443008U, "whois.nic.or.kr"),
            // 218.0.0.0/7
            Tuple.Create(3657433088U, 4261412864U, "whois.apnic.net"),
            // 220.64.0.0/11
            Tuple.Create(3695181824U, 4292870144U, "whois.nic.or.kr"),
            // 220.96.0.0/14
            Tuple.Create(3697278976U, 4294705152U, "whois.nic.ad.jp"),
            // 220.103.0.0/16
            Tuple.Create(3697737728U, 4294901760U, "whois.nic.or.kr"),
            // 220.104.0.0/13
            Tuple.Create(3697803264U, 4294443008U, "whois.nic.ad.jp"),
            // 220.149.0.0/16
            Tuple.Create(3700752384U, 4294901760U, "whois.nic.or.kr"),
            // 221.138.0.0/15
            Tuple.Create(3716808704U, 4294836224U, "whois.nic.or.kr"),
            // 221.140.0.0/14
            Tuple.Create(3716939776U, 4294705152U, "whois.nic.or.kr"),
            // 221.144.0.0/12
            Tuple.Create(3717201920U, 4293918720U, "whois.nic.or.kr"),
            // 221.160.0.0/13
            Tuple.Create(3718250496U, 4294443008U, "whois.nic.or.kr"),
            // 222.96.0.0/12
            Tuple.Create(3730833408U, 4293918720U, "whois.nic.or.kr"),
            // 222.112.0.0/13
            Tuple.Create(3731881984U, 4294443008U, "whois.nic.or.kr"),
            // 222.120.0.0/15
            Tuple.Create(3732406272U, 4294836224U, "whois.nic.or.kr"),
            // 222.122.0.0/16
            Tuple.Create(3732537344U, 4294901760U, "whois.nic.or.kr"),
            // 222.232.0.0/13
            Tuple.Create(3739746304U, 4294443008U, "whois.nic.or.kr"),
            // 220.0.0.0/6
            Tuple.Create(3690987520U, 4227858432U, "whois.apnic.net"),
        };
    }
}

