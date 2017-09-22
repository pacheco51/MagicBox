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
type SearchEngine =
   {   SearchEngineName: string
       SearchBaseUrl: string
       QueryParamName: string option
       NextPageUrl:string->string option
       OtherQueryParams: Map<string,string> option
       Parser: string ->  Result<SearchResult list, string>       
       HttpRequestData: RequestData
   }
