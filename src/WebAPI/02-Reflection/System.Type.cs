using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_Reflection
{
    internal class SystemType
    {
        public static void Main()
        {
			Type t = typeof(int);
			t.GetMethods();
			// MethodInfo[21] { [Int32 CompareTo(System.Object)], [Int32 CompareTo(Int32)], [Boolean Equals(System.Object)], [Boolean Equals(Int32)], [Int32 GetHashCode()], [System.String ToString()], [System.String ToString(System.String)], [System.String ToString(System.IFormatProvider)], [System.String ToString(System.String, System.IFormatProvider)], [Boolean TryFormat(System.Span`1[System.Char], Int32 ByRef, System.ReadOnlySpan`1[System.Char], System.IFormatProvider)], [Int32 Parse(System.String)], [Int32 Parse(System.String, System.Globalization.NumberStyles)], [Int32 Parse(System.String, System.IFormatProvider)], [Int32 Parse(System.String, System.Globalization.NumberStyles, System.IFormatProvider)], [Int32 Parse(System.ReadOnlySpan`1[System.Char], System.Globalization.NumberStyles, System.IFormatProvider)], [Boolean TryParse(System.String, Int32 ByRef)], [Boolean TryParse(System.ReadOnlySpan`1[System.Char], Int32 ByRef)], [Boolean TryParse(System.String, System.Globalization.NumberStyles, System.IFormatProvider, Int...

			Type t2 = "hello".GetType();
			Console.WriteLine(t2.FullName);
			// "System.String"

			t2.GetFields();
			// FieldInfo[1] { [System.String Empty] }

			t2.GetMethods();
			// MethodInfo[154] { [System.String Replace(System.String, System.String)], [System.String[] Split(Char, System.StringSplitOptions)], [System.String[] Split(Char, Int32, System.StringSplitOptions)], [System.String[] Split(Char[])], [System.String[] Split(Char[], Int32)], [System.String[] Split(Char[], System.StringSplitOptions)], [System.String[] Split(Char[], Int32, System.StringSplitOptions)], [System.String[] Split(System.String, System.StringSplitOptions)], [System.String[] Split(System.String, Int32, System.StringSplitOptions)], [System.String[] Split(System.String[], System.StringSplitOptions)], [System.String[] Split(System.String[], Int32, System.StringSplitOptions)], [System.String Substring(Int32)], [System.String Substring(Int32, Int32)], [System.String ToLower()], [System.String ToLower(System.Globalization.CultureInfo)], [System.String ToLowerInvariant()], [System.String ToUpper()], [System.String ToUpper(System.Globalization.CultureInfo)], [System.String ToUpperInvariant()], [System.String Trim()],...

			var a = typeof(string).Assembly;
			Console.WriteLine(a);
			// [System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]

			var types = a.GetTypes();
			Console.WriteLine(types[10]);
			// [Interop+Kernel32+FINDEX_SEARCH_OPS]

			Console.WriteLine(types[10].FullName);
			// "Interop+Kernel32+FINDEX_SEARCH_OPS"

			types[10].GetMethods();
			// MethodInfo[10] { [Boolean Equals(System.Object)], [Boolean HasFlag(System.Enum)], [Int32 CompareTo(System.Object)], [Int32 GetHashCode()], [System.String ToString()], [System.String ToString(System.String, System.IFormatProvider)], [System.String ToString(System.String)], [System.String ToString(System.IFormatProvider)], [System.TypeCode GetTypeCode()], [System.Type GetType()] }

			var t3 = Type.GetType("System.Int64");
			Console.WriteLine(t3.FullName);
			// "System.Int64"

			t3.GetMethods();
			// MethodInfo[21] { [Int32 CompareTo(System.Object)], [Int32 CompareTo(Int64)], [Boolean Equals(System.Object)], [Boolean Equals(Int64)], [Int32 GetHashCode()], [System.String ToString()], [System.String ToString(System.IFormatProvider)], [System.String ToString(System.String)], [System.String ToString(System.String, System.IFormatProvider)], [Boolean TryFormat(System.Span`1[System.Char], Int32 ByRef, System.ReadOnlySpan`1[System.Char], System.IFormatProvider)], [Int64 Parse(System.String)], [Int64 Parse(System.String, System.Globalization.NumberStyles)], [Int64 Parse(System.String, System.IFormatProvider)], [Int64 Parse(System.String, System.Globalization.NumberStyles, System.IFormatProvider)], [Int64 Parse(System.ReadOnlySpan`1[System.Char], System.Globalization.NumberStyles, System.IFormatProvider)], [Boolean TryParse(System.String, Int64 ByRef)], [Boolean TryParse(System.ReadOnlySpan`1[System.Char], Int64 ByRef)], [Boolean TryParse(System.String, System.Globalization.NumberStyles, System.IFormatProvider, Int...

			var t4 = Type.GetType("System.Collections.Generic.List`1");
			Console.WriteLine(t4.FullName);
			// "System.Collections.Generic.List`1"

			t4.GetFields();
			// FieldInfo[0] { }

			t4.GetMethods();
			// MethodInfo[55] { [Int32 get_Capacity()], [Void set_Capacity(Int32)], [Int32 get_Count()], [T get_Item(Int32)], [Void set_Item(Int32, T)], [Void Add(T)], [Void AddRange(System.Collections.Generic.IEnumerable`1[T])], [System.Collections.ObjectModel.ReadOnlyCollection`1[T] AsReadOnly()], [Int32 BinarySearch(Int32, Int32, T, System.Collections.Generic.IComparer`1[T])], [Int32 BinarySearch(T)], [Int32 BinarySearch(T, System.Collections.Generic.IComparer`1[T])], [Void Clear()], [Boolean Contains(T)], [System.Collections.Generic.List`1[TOutput] ConvertAll[TOutput](System.Converter`2[T,TOutput])], [Void CopyTo(T[])], [Void CopyTo(Int32, T[], Int32, Int32)], [Void CopyTo(T[], Int32)], [Boolean Exists(System.Predicate`1[T])], [T Find(System.Predicate`1[T])], [System.Collections.Generic.List`1[T] FindAll(System.Predicate`1[T])], [Int32 FindIndex(System.Predicate`1[T])], [Int32 FindIndex(Int32, System.Predicate`1[T])], [Int32 FindIndex(Int32, Int32, System.Predicate`1[T])], [T FindLast(System.Predicate`1[T])], [Int32 Fin...


		}
	}
}
