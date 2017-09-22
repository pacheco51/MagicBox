module Google

let Google =
    {
      SearchEngineName = "Google"  
      SearchBaseUrl = "https://www.google.com/search"  
      QueryParamName = Some "q"
      NextPageParamName = Some "start"
      NextPageValue = function PageInt page->PageInt ((page-1)*10)|>Some | _ -> None
      PagesToScrape = Some 10
      AllOtherQueryParams = None
      Parser = (fun html-> Error "Not Implemented")
      IsBlocked = None
      HttpRequestData = DefaultRequestData
     }

