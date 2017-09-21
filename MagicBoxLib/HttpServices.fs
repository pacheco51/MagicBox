[<AutoOpen>]
module  HttpServices
open System.Net
open FSharp.Data
open HttpDomains

let FetchResponse (url:string) (requestData:RequestData)=
    async{
           return! Http.AsyncRequest(url, headers = requestData.Headers, cookieContainer = requestData.Cookies, timeout = (int requestData.Delay),silentHttpErrors = true,
                                            customizeHttpRequest = (fun req -> req.Proxy <- requestData.Proxy.GetWebProxy()
                                                                               req))
        }

