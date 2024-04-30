using System.Reflection.Metadata;

var t = typeof(bool);
var b = Activator.CreateInstance(t);
Console.WriteLine(b);

var b2 = Activator.CreateInstance<bool>;
Console.WriteLine(b2);

var wc = Activator.CreateInstance("System", "System.Timers.Timer");
Console.WriteLine(wc);
Console.WriteLine(wc?.Unwrap());

var allType = Type.GetType("System.Collections.ArrayList");
Console.WriteLine(allType);

var alCtor = allType?.GetConstructor(Array.Empty<Type>());
Console.WriteLine(alCtor);

var al = alCtor?.Invoke(Array.Empty<object>());
Console.WriteLine(al);

var st = typeof(string);
var ctor = st.GetConstructor(new[] { typeof(char[]) });
Console.WriteLine(ctor);

var str = ctor?.Invoke(new object?[]
{
    new [] {'t', 'e', 's', 't'}
});
Console.WriteLine(str);

var listType = Type.GetType("System.Collection.Generic.List`1");
Console.WriteLine(listType);

var listOfIntType = listType?.MakeGenericType(typeof(int));

Console.WriteLine(listOfIntType);

var listOfIntCtor = listOfIntType?.GetConstructor(Array.Empty<Type>());
Console.WriteLine(listOfIntCtor);

var theList = listOfIntCtor?.Invoke(Array.Empty<object>());
Console.WriteLine(theList);

var charType = typeof(char);
Array.CreateInstance(charType, 10);

Console.WriteLine(charType);
var charArrayType = charType.MakeArrayType();
Console.WriteLine(charArrayType);

Console.WriteLine(charArrayType.FullName);
var charArrayCtor = charArrayType.GetConstructor(new[] { typeof(int) });
Console.WriteLine(charArrayCtor);

var arr = charArrayCtor?.Invoke(new object?[] { 20 });

Console.WriteLine(arr);
Console.WriteLine(arr?.GetType().Name);

char[]? arr2 = (char[])arr!;
Console.WriteLine(arr2);

arr2[1] = 'Z';
Console.WriteLine(arr2);