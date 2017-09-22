module Google

let Google =
    {
      SearchEngineName = "Google"  
      SearchBaseUrl = "https://www.google.com/search"  
      QueryParamName = Some "q"
      NextPageUrl = fun html -> None
      OtherQueryParams = None
      Parser = (fun html-> Error "Not Implemented")
      HttpRequestData = DefaultRequestData
     }

