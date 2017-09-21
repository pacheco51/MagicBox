[<AutoOpen>]
module  HttpServices
open System
open System.Net
open FSharp.Data
open HttpDomains
open FSharp.Data.HtmlAttribute

let Fetch (url:string) (requestData:RequestData)=
    let headers = GetDefaultHeader()
    async{
           let req = Http.AsyncRequest(url, headers = requestData.Headers, cookieContainer = requestData.Cookies,
                                       customizeHttpRequest= fun req ->
                                                                 req.Proxy <- requestData.Proxy.GetWebProxy()
                                       ) 
           
        }

