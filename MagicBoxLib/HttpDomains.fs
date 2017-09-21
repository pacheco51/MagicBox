module HttpDomains
open System
open System.Net

type Ip = string
type Port = int 
type UserName = string
type Password = string

type SearchProxy =
    |PrivateProxy of Ip * Port * UserName * Password
    |PublicProxy of Ip * Port
    member this.GetWebProxy() =
        match this with
        |PrivateProxy (ip,port,name, pass)-> 
            let p = new WebProxy(ip,port)
            p.Credentials <- new NetworkCredential(name,pass)
            p
        |PublicProxy (ip,port) -> WebProxy(ip,port)
             
let GetRandomUserAgents =
    let UserAgents = [|
        "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.86 Safari/537.36";
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_1) AppleWebKit/601.2.7 (KHTML, like Gecko) Version/9.0.1 Safari/601.2.7";
        "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.86 Safari/537.36";
        "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36";
        "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
        "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36";
        |]
    let rnd = new Random()
    let len = UserAgents.Length
    fun()-> UserAgents.[rnd.Next(len)]

let GetDefaultHeaders = 
    let headers = 
        [
          "Accept","text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"
          "Accept-Language","en-US,en;q=0.5"
          "Accept-Encoding", "gzip, deflate"
          "Connection", "keep-alive"
        ]
    fun()->headers

type [<Measure>] DelayInMs

type RequestData = 
    {
       Cookies:CookieContainer
       Proxy:SearchProxy     
       Headers:string*string list
       Delay: int<DelayInMs> 

     }