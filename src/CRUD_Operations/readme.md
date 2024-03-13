# CRUD Operations Cheat Sheet

## Route Tokens
> The route tokens [controller], [action] can be used to apply common-patterned routes for all action methods.

> The route of controller acts as a prefix for the route of actions

![route_tokens_1](assets/route_tokens_1.png)

![route_tokens_2](assets/route_tokens_2.png)

## Attribute Routing
> [Route] attribute specifies route for an action method or controller.

> The route of controller acts as a prefix for the route of actions.

### [Route] - Attribute Routing

![attribute_routing_1](assets/attribute_routing_1.png)
![attribute_routing_2](assets/attribute_routing_2.png)

# Interview Questions 

## Explain how attribute-based routing works?
The route of any action method is specified in the `[Route]` attribute
The `[route]` of the controller acts as a prefix for the route of its action methods i.e:
```c#
[Route("[controller]/[action]")]
```
## “You can map routes to endpoints explicitly (attribute routing) or through convention (convention routing); which do you prefer and why?”
Developers must be careful when using attribute routing. Whether you rename the `controller` or the `action` method, the current route to reach that `action` method will
surely change as well
I prefer to use convention routing as I can have the control on every change in the route.

## “You have a page with a form, but when you submit, nothing occurs. How would you go about debugging the issue?”

## How do you implement buffering and streaming file uploading files into asp.net core app?

## What is the difference between ViewModel and DTO?