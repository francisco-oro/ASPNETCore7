# View Components Cheat Shet
## View Components
View Component is a combination of a class (derived from Microsoft.AspNetCore.ViewComponent) that supplied data, and a partial view to render that data.


![viewcomponents](assets/viewcomponents.png)



### Invoking View Component
```c#
@await Component.InvokeAsync("view component name");
--or--
<vc:view-component-name />
```

### View Components

- View component renders a chunk rather than a whole response.

- Includes the same separation-of-concerns and testability benefits found with a controller and view.

- Should be either suffixed with the word "ViewComponent" or should have [ViewComponent] attribute.

- Optionally, it can inherit from System.AspNetCore.Mvc.ViewComponent.





## View Components with ViewData
The ViewComponent class can share ViewData object to the ViewComponent view.


![viewcomponents_with_viewdata)](assets/viewcomponents_with_viewdata).png)




## Strongly Typed ViewComponent
Strongly Typed ViewComponent's view is tightly bound to a specified model class.

So, it gets all the benefits of a strongly typed view.

![stronglytyped_viewcomponent](assets/stronglytyped_viewcomponent.png)







## ViewComponents with Parameters
You can supply one or more parameters to the view component class.

The parameters are received by InvokeAsync method of the view component class.

All the parameters of view component are mandatory (must supply a value).



![viewcomponents_with_parameters](assets/viewcomponents_with_parameters.png)



## Invoking ViewComponent with parameters
```c#
@await Component.InvokeAsync("view component name", new { param = value });

-- or --

<vc:view-component-name param="value" />
```




## ViewComponentResult
ViewComponent can represent the content of a view component .

Generally useful to fetch view component's content into the browser, by making an asynchronous request (XMLHttpRequest / fetch request) from the browser.



![viewcomponentresult](assets/viewcomponentresult.png)


```c#
return new ViewComponentResult() { ViewName = "view component name", Arguments = new { param1 = value, param2 = value } };
[or]
return ViewComponent("view component name", new { param1 = value, param2 = value } });
```

# Interview Questions
## Describe view components?
ViewComponent was introduced in ASP.NET Core MVC. It can do everything that a partial view can and can do even more. ViewComponents are completely self-contained objects that consistently render html from a razor view. ViewComponents are very powerful UI building blocks of the areas of application which are not directly accessible for controller actions.

Let's suppose we have a page of social icons and we display icons dynamically. We have separate settings for color, urls and names of social icons and we have to render icons dynamically. It’s good to create it as a ViewComponent, because we can pass data to ViewComponent dynamically.

```c#
public class MyViewComponent : ViewComponent
{
 public async Task<IViewComponentResult> InvokeAsync()
 {
  return View(model); //invokes the view of ViewComponent
 }
}
```

## When would you use ViewComponent over a partial view?
If a view needs to render partial view without needing to pass any model data, partial view could be enough.

But if we would like to pass model data (generally, data from database), view component is recommended.



View Components are completely self-contained objects that consistently render html from a Razor view. View Components offers separation of concerns as they don’t depend on controllers.

