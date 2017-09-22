[<AutoOpen>]
module SearchDomains

type SearchResult =
 {
   SearchEngineName :string
   SearchKeyword:string
   Url:string
   LinkText:string 
   ResultPostion:int option
   HtmlSnippet:string option
  }

type IntOrString =
   |PageString of string
   |PageInt of int

type SearchEngine =
   {   SearchEngineName: string
       SearchBaseUrl: string
       QueryParamName: string option
       NextPageParamName: string option
       NextPageValue: IntOrString -> IntOrString option
       PagesToScrape: int option
       AllOtherQueryParams: Map<string,string> option
       Parser: string ->  Result<SearchResult list, string>       
       IsBlocked: (string -> bool) option
       HttpRequestData: RequestData
   }
