# Partial Views CheatS Sheet

## Partial Views
Partial view is a razor markup file (.cshtml) that can't be invoked individually from the controller; but can be invoked from any view within the same web application.


![partialviews](assets/partialviews.png)



## Invoking Partial Views
`<partial name="partial view name" />`

Returns the content to the parent view.



`@await Html.PartialAsync("partial view name")`

Returns the content to the parent view.



`@{ await Html.RenderPartialAsync("partial view name"); }`

Streams the content to the browser.





## Partial Views with ViewData
When partial view is invoked, it receives a copy of the parent view's ViewData object.

So, any changes made in the ViewData in the partial view, do NOT effect the ViewData of the parent view.

Optionally, you can supply a custom ViewData object to the partial view, if you don't want the partial view to access the entire ViewData of the parent view.

![partialviews_with_viewdata](assets/partialviews_with_viewdata.png)





### Invoking Partial Views with View Data

`@{ await Html.RenderPartialAsync("partial view name", ViewData); }`

-- or --

`<partial name="partial view name" view-data="ViewData" />`





## Strongly Typed Partial Views
Strongly Typed Partial View is a partial view that is bound to a specified model class.

So, it gets all the benefits of a strongly typed view.


![partialviews_stronglytyped](assets/partialviews_stronglytyped.png)


### Invoking Strongly-Typed Partial View

`@{ await Html.RenderPartialAsync("partial view name", Model); }`

-- or --

`<partial name="partial view name" model="Model" />`





## PartialViewResult
PartialViewResult can represent the content of a partial .

Generally useful to fetch partial view's content into the browser, by making an asynchronous request (XMLHttpRequest / fetch request) from the browser.

![partialviewresult](assets/partialviewresult.png)



`return new PartialViewResult() { ViewName = "partial view name", Model = model };`

[or]

`return PartialView("partial view name", model);`

# Interview Questions

## Explain partial views in asp.net core?
A partial view is a Razor markup file (.cshtml) that renders HTML output within another markup file's rendered output.

Partial views are an effective way to:

- Break up large views into smaller components.

- Reduce the duplication of common markup content across views.



You will invoke the partial view either by using the <partial> tag helper or an asynchronous html helper called Html.PartialAsync() or Html.RenderPartialAsync().
## Explain the difference between PartialAsync() and RenderPartialAsync()
### 1:

- The Html.PartialAsync() method returns the content to the parent view.

- The Html.RenderPartialAsync() method streams the content to the browser; so it offers faster performance is there is large amount of content in the partial view.

2:

- The return type of PartialAsync() method is IHtmlContent. Hence its output can be stored in a variable.

- The return type of RenderPartialAsync() method is void. So you can’t store its output into a variable. It will be directly streamed to the browser.