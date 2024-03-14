# Tag Helpers Section Cheat Sheet (PPT)
## Tag Helpers
> Tag helpers are the classes that can be invoked as an html tag or html attribute.

> They generate a html tag or adds values into attributes of existing html tags.
```c#
<input asp-for="ModelProperty">
<input type="text" name="ModelProperty" id="ModelProperty" value="ModelValue" />
```

### Tag Helpers for <a>, <form>

- asp-controller

- asp-action

- asp-route-x

- asp-route

- asp-area



### Tag Helpers for <input>, <textarea>, <label>

- asp-for



### Tag Helpers for <select>

- asp-for

- asp-items





### Binding

The form tags such as <input>, <label>, <textarea>, <select> can be bound with specific model properties.

It applies model property name to "name" and "id" attributes of <input> tag.



### Url Generation

Route URLs' will be re-generated for <a> and <form> tags;

It generates the url as "controller/action" pattern.





### Tag Helpers for <img>

- asp-append-version



### Tag Helpers for <span>

- asp-validation-for



### Tag Helpers for <script>

- asp-fallback-src

- asp-fallback-test



### Tag Helpers for <div>

- asp-validation-summary

### Tag Helpers for <form>
```c#
<form asp-controller="ControllerName" asp-action="ActionName">
</form>
```
will be converted as:
```c#
<form action="~/ControllerName/ActionName">
</form>
```
### asp-controller and asp-action

Generates route url for the specified action method with "controller/action" route pattern.





Tag Helpers for <a>
<a asp-controller="ControllerName" asp-action="ActionName"> </a>

will be converted as:

<a href="~/ControllerName/ActionName"> </a>



asp-controller and asp-action

Generates route url for the specified action method with "controller/action" route pattern.





Tag Helpers for <a> and <form>
<a asp-controller="ControllerName" asp-action="ActionName" asp-route-parameter="value" > </a>

will be converted as:

<a href="~/ControllerName/ActionName/value-of-parameter"> </a>



asp-route-x

Specifies value for a route parameter, which can be a part of the route url.



Tag Helpers for <input>, <textarea>, <select>
<input asp-for="ModelProperty" />

will be converted as:

<input type="text" name="ModelProperty" id="ModelProperty" value="ModelValue" data-val-rule="ErrorMessage" />



asp-for

Generates "type", "name", "id", "data-validation" attributes for the <input>, <textarea>, <select> tags.





Tag Helpers for <label>
<label asp-for="ModelProperty"> </label>

will be converted as:

<label for="ModelProperty"> </label>



asp-for

Generates "for" attribute for the <label>.





Client Side Validations
Data annotations on model properties

[Required]
publicDataTypePropertyName { get; set; }


"data-*" attributes in html tags [auto-generated with "asp-for" helper]

<input data-val="true" data-required="ErrorMessage" />



Import jQuery Validation Scripts

https://cdnjs.cloudflare.com/ajax/libs/ jquery/3.6.0/jquery.min.js
https://cdnjs.cloudflare.com/ajax/libs/ jquery-validate/1.19.3/jquery.validate.min.js
https://cdnjs.cloudflare.com/ajax/libs/ jquery-validation-unobtrusive/3.2.12/ jquery.validate.unobtrusive.min.js


Tag Helpers for <img>
<img src="~/FilePath" asp-append-version="true" />
<img src="/FilePath?v=HashOfImageFile" />


asp-append-version

Generates SHA512 hash of the image file as query string parameter appended to the file path.

It REGENERATES a new hash every time when the file is changed on the server. If the same file is requested multiple times, file hash will NOT be regenerated.



Tag Helpers for <script>
<script src="CDNUrl" asp-fallback-src="~/LocalUrl" asp-fallback-test="object"> </script>
<script src="CDNUrl"> </script>
<script> object || document.write("<script src='/LocalUrl'></script>"); </script>


asp-fallback-src

It makes a request to the specified CDNUrl at the "src" attribute.

It checks the value of the specified object at the "asp-fallback-test" tag helper.

If its value is null or undefined (means, the script file at CDNUrl is not loaded), then it makes another request to the LocalUrl through another script tag.

