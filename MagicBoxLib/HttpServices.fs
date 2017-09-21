[<AutoOpen>]
module  HttpServices
open System.Net
open FSharp.Data
open HttpDomains

let FetchResponse (url:string) (requestData:RequestData)=
    async{
           match requestData.Method with
           |Get -> return! Http.AsyncRequest(url, headers = requestData.Headers, httpMethod = HttpMethod.Get, cookieContainer = requestData.Cookies, timeout = (int requestData.Delay),silentHttpErrors = true,
                                            customizeHttpRequest = (fun req -> req.Proxy <- requestData.Proxy.GetWebProxy()
                                                                               req))
           |Post ->let formValues = requestData.UploadValues |> FormValues
                   return! Http.AsyncRequest(url, headers = requestData.Headers, httpMethod = HttpMethod.Post,body = formValues, cookieContainer = requestData.Cookies, timeout = (int requestData.Delay),silentHttpErrors = true,
                                         customizeHttpRequest = (fun req -> req.Proxy <- requestData.Proxy.GetWebProxy()
                                                                            req))
        }

