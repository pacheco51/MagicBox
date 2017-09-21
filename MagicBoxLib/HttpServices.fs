[<AutoOpen>]
module  HttpServices
open System
open System.Net
open FSharp.Data
open HttpDomains
open System.Threading

let Fetch (url:string) (requestData:RequestData)=
    async{
           let! resp = Http.AsyncRequest(url, headers = requestData.Headers, cookieContainer = requestData.Cookies, timeout = (int requestData.Delay),
                                         customizeHttpRequest = (fun req -> req.Proxy <- requestData.Proxy.GetWebProxy()
                                                                            req))
           return resp.StatusCode,resp.Body          
        }

