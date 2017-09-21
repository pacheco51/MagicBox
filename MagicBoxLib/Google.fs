module Google


//type SearchEngine =
//   {   SearchEngineName: string
//       SearchBaseUrl: string
//       QueryParam: string * string
//       NextPageParamName: string
//       NextPageValue: int -> int  
//       PageNum: int
//       AllOtherQueryParams: Map<string,string> 
//       Results: string -> SearchResult list
//   }


let Google =
    {
      SearchEngineName = "Google"  
      SearchBaseUrl = "https://www.google.com/search"  
      QueryParamName = "q"
      NextPageParamName = "start"
      NextPageValue = function PageInt page->PageInt ((page-1)*10)|>Some | _ -> None
      PagesToScrape = 10
      AllOtherQueryParams = None
      Parser = (fun html-> None)
     }

