module Google
open System
open System.Net
open System.IO
open FSharp.Data

type Search = { 
                Name: string
                SearchBaseUrl : string
                QueryName : string
                QueryParams : (string*string)list
                NextPage : string
                Parent : string
                Children : (string*string)list
                CurateLink : string
               }


type SearchProvider = JsonProvider<Sample = "Json/searchsites.json", EmbeddedResource="MagicBoxLib.dll, searchsites.json">

let test = SearchProvider.GetSamples()
test|>Seq.iter (fun x-> printfn "%s" x.Name)

(*
type SearchResult =
 {
   Url:string
   LinkText:string 
   ResultPosition:int 
   SnippetHtml:string
  }
type SearchEngine =
   {   SearchEngineName: string
       SearchBaseUrl: string
       QueryParamName: string option
       NextPageUrlFunc:HtmlDocument->string option
       OtherQueryParams: Map<string,string> option
       Parser: HtmlDocument ->  Result<SearchResult list, string>       
       HttpRequestData: RequestData
   }

let GetNextPageUrl (hrefSelector:string) =
    fun (doc:HtmlDocument) -> 
      let nodes = doc.CssSelect(hrefSelector)
      if List.length nodes > 0 then 
        (nodes |>List.last).AttributeValue("href")|>Some else None

let CreateParser (topTag:string) (anchor:string) (snippet:string) (curateLink:(string->string) option) (isBlocked: (HtmlDocument->bool) option)=
   fun (doc:HtmlDocument)->
       let nodes = doc.CssSelect(topTag)
       if(nodes.IsEmpty) then
            match isBlocked with
            |Some f -> if(f doc) then 
                         Error "Blocked"
                       else
                         Error "Couldn't found results..."
            |None -> Error "Couldn't found results..."
       else
         Ok(nodes|>List.mapi(fun i n-> let a = List.head (n.CssSelect(anchor))
                                       let href = a.AttributeValue("href")
                                       let link =
                                         match curateLink with
                                          |Some f -> f href
                                          |None -> href
                                       let position = i+1
                                       let linkText = a.InnerText()
                                       let sn = List.head (n.CssSelect(snippet))
                                       let snippet = sn.ToString()
                                       {
                                         Url = link
                                         LinkText = linkText
                                         ResultPosition = position
                                         SnippetHtml = snippet
                                       }
                             ))
    
let Google =
    {
      SearchEngineName = "Google"  
      SearchBaseUrl = "https://www.google.com/search"  
      QueryParamName = Some "q"
      NextPageUrlFunc = GetNextPageUrl "div#foot td.b a"
      OtherQueryParams = None
      Parser = CreateParser "div#ires div.g" "h3 > a" "span.st" (Some(fun href-> href.[7..(href.IndexOf("&sa="))])) None
      HttpRequestData = DefaultRequestData
     }

let Bing = {
      SearchEngineName = "Bing"  
      SearchBaseUrl = "http://www.bing.com/search"  
      QueryParamName = Some "q"
      NextPageUrlFunc = GetNextPageUrl("a.sb_pagN")
      OtherQueryParams = None
      Parser = CreateParser "li.b_algo" "h2 > a" "div > p"  None None 
      HttpRequestData = DefaultRequestData
     }
     
let Yahoo = {
      SearchEngineName = "Yahoo"  
      SearchBaseUrl = "https://search.yahoo.com/search"  
      QueryParamName = Some "p"
      NextPageUrlFunc = GetNextPageUrl("a.next")
      OtherQueryParams = None
      Parser = CreateParser "div.dd.algo" "a.ac-algo" "div.compText.aAbs p" (Some(fun href ->
                                                                                        if(href.StartsWith("http")) then href
                                                                                        else
                                                                                          let index = href.IndexOf("http")
                                                                                          let lastIndex = href.IndexOf("/")
                                                                                          href.[index..lastIndex] 
                                                                                       )) None 
      HttpRequestData = DefaultRequestData
     }

let Yandex = {
      SearchEngineName = "Yandex"  
      SearchBaseUrl = "https://www.yandex.com/search/"  
      QueryParamName = Some "text"
      NextPageUrlFunc = GetNextPageUrl("a.pager__item.pager__item_kind_next")
      OtherQueryParams = None
      Parser = CreateParser "li.serp-item > div.organic" "h2 > a" "div.organic__text" None None 
      HttpRequestData = DefaultRequestData
     }

let DuckDuckGo =
    {
      SearchEngineName = "DuckDuckGo"  
      SearchBaseUrl = "https://duckduckgo.com/html"  
      QueryParamName = Some "q"
      NextPageUrlFunc = fun doc -> let nodes = doc.CssSelect("div.nav-link > form > input")
                                   if List.length nodes >= 0 
                                   then (("&",(nodes |>List.choose(fun n-> match (n.TryGetAttribute("name")), (n.TryGetAttribute("value")) with
                                                                           |Some name, Some v ->Some(sprintf "%s=%s" (name.Value()) (WebUtility.UrlEncode(v.Value())))
                                                                           |_->None
                                                               )))|>String.Join
                                         )|> Some
                                   else None
      OtherQueryParams = None
      Parser = CreateParser "div.links_main.links_deep.result__body" "a.result__a" "a.result__snippet" None None 
      HttpRequestData = DefaultRequestData
     }
    
let Ask =
    {
      SearchEngineName = "Ask"  
      SearchBaseUrl = "http://www.ask.com/web"  
      QueryParamName = Some "q"
      NextPageUrlFunc = GetNextPageUrl("ul.PartialWebPagination a") 
      OtherQueryParams = None
      Parser = CreateParser "div.PartialSearchResults-item" "a.PartialSearchResults-item-title-link.result-link" "p.PartialSearchResults-item-abstract" None None 
      HttpRequestData = DefaultRequestData
     }
    
let Baidu =
    {
      SearchEngineName = "Baidu"  
      SearchBaseUrl = "http://www.baidu.com/s"  
      QueryParamName = Some "wd"
      NextPageUrlFunc = GetNextPageUrl("div#page a")
      OtherQueryParams = None
      Parser = CreateParser "div.result" "h3 a" ".c-abstract" None None 
      HttpRequestData = DefaultRequestData
     }

let Sogou =
    {
      SearchEngineName = "Sogou"  
      SearchBaseUrl = "http://www.sogou.com/web"  
      QueryParamName = Some "query"
      NextPageUrlFunc = GetNextPageUrl("a.sogou_next")
      OtherQueryParams = None
      Parser = CreateParser "div.rb" "h3 a" "div.ft" None None 
      HttpRequestData = DefaultRequestData
     }
     
let Amazon =
     {
      SearchEngineName = "Amazon"  
      SearchBaseUrl = "https://www.amazon.com/s/"  
      QueryParamName = Some "field-keywords"
      NextPageUrlFunc = GetNextPageUrl("a[title='Next Page']")
      OtherQueryParams = None
      Parser = CreateParser "li[id^='result']" "a[title]" "a[title]" None None 
      HttpRequestData = DefaultRequestData
     }
     
let Rambler =
     {
      SearchEngineName = "Rambler"  
      SearchBaseUrl = "https://nova.rambler.ru/search"  
      QueryParamName = Some "query"
      NextPageUrlFunc = GetNextPageUrl("a.b-paging__link")
      OtherQueryParams = None
      Parser = CreateParser "div.b-serp-item" "a.b-serp-item__link" "p.b-serp-item__snippet" None None 
      HttpRequestData = DefaultRequestData
     }
*)     