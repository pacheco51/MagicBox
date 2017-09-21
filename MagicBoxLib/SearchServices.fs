[<AutoOpen>]
module SearchService

open System
open System.Net

//let GetQueryParams ((qp,kw):Query) (npp:NextPageParamName)  (nextPageValue: NextPageValue) (pageNum: PageNum) (aoqps: AllOtherQueryParams) =
//    let x = sprintf "&%s=%s&%s=%d" qp kw npp (nextPageValue(pageNum))
//    let y = aoqps|>Seq.map(fun p-> sprintf "%s=%s" (p.Key) (WebUtility.UrlEncode(p.Value)))
//    let str = String.Join("&", y)
//    x+str
    
//let Search (engine:HtmlSearchEngine) =
//    let url, query, npp, npv, pageNum, aoqps, cs, d, p = engine
//    let q = GetQueryParams query npp npv pageNum aoqps 
//    let searchUrl = sprintf "%s/%s" url q 
//    ()

let Scrape (engine:SearchEngine) (data:RequestData) =
    ()