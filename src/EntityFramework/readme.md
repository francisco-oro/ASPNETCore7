# Entity Framework - Section Cheat Sheet

## Introduction to EntityFrameworkCore
>  is light-weight, extensible and cross-platform framework for accessing databases in .NET applications.

> It is the most-used database framework for Asp.Net Core Apps.

![entityframework](assets/entityframework.png)


### EFCore Models

![efcore_models](assets/efcore_models.png)



## Pros & Cons of EntityFrameworkCore
### Shorter Code

The CRUD operations / calling stored procedures are done with shorter amount of code than ADO.NET.



### Performance

EFCore performs slower than ADO.NET.

So ADO.NET or its alternatives (such as Dapper) are recommended for larger & high-traffic applications.



### Strongly-Typed

The columns as created as properties in model class.

So the Intellisense offers columns of the table as properties, while writing the code.

Plus, the developer need not convert data types of values; it's automatically done by EFCore itself.





## Approaches in Entity Framework Core
### EFCore Approaches


![efcore_approaches](assets/efcore_approaches.png)



## Pros and Cons of EFCore Approaches


### CodeFirst Approach

Suitable for newer databases.

Manual changes to DB will be most probably lost because your code defines the database.

Stored procedures are to be written as a part of C# code.

Suitable for smaller applications or prototype-level applications only; but not for larger or high data-intense applications.



### DbFirst Approach

Suitable if you have an existing database or DB designed by DBAs, developed separately.

Manual changes to DB can be done independently.

Stored procedures, indexes, triggers etc., can be created with T-SQL independently.

Suitable for larger applications and high data-intense applications.





## DbContext and DbSet

### DbContext

An instance of DbContext is responsible to hold a set of DbSets' and represent a connection with database.



### DbSet

Represents a single database table; each column is represented as a model property.



### Add DbContext as Service in Program.cs:
```c#
builder.Services.AddDbContext<DbContextClassName>( options => {
 options.UseSqlServer();
}
);
```



## Code-First Migrations

### Migrations

Creates or updates database based on the changes made in the model.



### in Package Manager Console (PMC):

`Add-Migration MigrationName`

//Adds a migration file that contains C# code to update the database



`Update-Database -Verbose`

//Executes the migration; the database will be created or table schema gets updated as a result.







## Seed Data
in DbContext:

`modelBuilder.Entity<ModelClass>().HasData(entityObject);`

It adds initial data (initial rows) in tables, when the database is newly created.





## EF CRUD Operations - Query


### SELECT - SQL
```sql
SELECT Column1, Column2 FROM TableName
 WHERE Column = value
 ORDER BY Column
```

### LINQ Query:
```c#
_dbContext.DbSetName
 .Where(item => item.Property == value)
 .OrderBy(item => item.Property)
 .Select(item => item);
 
//Specifies condition for where clause
//Specifies condition for 'order by' clause
//Expression to be executed for each row
```



## EF CRUD Operations - Insert
### INSERT - SQL
```sql
INSERT INTO TableName(Column1, Column2) VALUES (Value1, Value2)
```


### Add:
```c#
_dbContext.DbSetName.Add(entityObject);
//Adds the given model object (entity object) to the DbSet.
```

### SaveChanges()
```c#
_dbContext.SaveChanges();
//Generates the SQL INSERT statement based on the model object data and executes the same at database server.
```





## EF CRUD Operations - Delete
### DELETE - SQL
```sql
DELETE FROM TableName WHERE Condition
```


### Remove:
```c#
_dbContext.DbSetName.Remove(entityObject);
//Removes the specified model object (entity object) to the DbSet.
```

### SaveChanges()
```c#
_dbContext.SaveChanges();
//Generates the SQL DELETE statement based on the model object data and executes the same at database server.
```




## EF CRUD Operations - Update
### UPDATE - SQL
```sql
UPDATE TableName SET Column1 = Value1, Column2 = Value2 WHERE PrimaryKey = Value
```


### Update:
```c#
entityObject.Property = value;
//Updates the specified value in the specific property of the model object (entity object) to the DbSet.
```

### SaveChanges()
```c#
_dbContext.SaveChanges();
//Generates the SQL UPDATE statement based on the model object data and executes the same at database server.
```





## How EF Query Works?
### Workflow of Query Processing in EF

![query_processing_ef](assets/query_processing_ef.png)



## EF - Calling Stored Procedures
### Stored Procedure for CUD (INSERT | UPDATE | DELETE):
```c#
int DbContext.Database.ExecuteSqlRaw(
 string sql,
 params object[] parameters)
 
//Eg: "EXECUTE [dbo].[StoredProcName] @Param1 @Parm2
//A list of objects of SqlParameter type
```

### Stored Procedure for Retrieving (Select):
```c#
IQueryable<Model> DbSetName.FromSqlRaw(
 string sql,
 paramsobject[] parameters)
 
//Eg: "EXECUTE [dbo].[StoredProcName] @Param1 @Parm2"
//A list of objects of SqlParameter type
```



### Creating Stored Procedure (SQL Server)
```sql
CREATE PROCEDURE [schema].[procedure_name]
(@parameter_name data_type, @parameter_name data_type)
AS BEGIN
 statements
END
```


## Advantages of Stored Procedure
### Single database call

You can execute multiple / complex SQL statements with a single database call.

As a result, you'll get:

- Better performance (as you reduce the number of database calls)

- Complex database operations such as using temporary tables / cursors becomes easier.



### Maintainability

The SQL statements can be changed easily WITHOUT modifying anything in the application source code (as long as inputs and outputs doesn't change)





## [Column] Attribute
### Model class
```c#
public class ModelClass
{
  [Column("ColumnName", TypeName = "datatype")]
  public DataType PropertyName { get; set; }
 
  [Column("ColumnName", TypeName = "datatype")]
  publicDataTypePropertyName { get; set; }
}
Specifies column name and data type of SQL Server table.
```



## EF - Fluent API
### DbContext class
```c#
public class CustomDbContext : DbContext
{
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    //Specify table name (and schema name optionally) to be mapped to the model class
    modelBuilder.Entity<ModelClass>( ).ToTable("table_name", schema: "schema_name");
 
    //Specify view name (and schema name optionally) to be mapped to the model class
    modelBuilder.Entity<ModelClass>( ).ToView("view_name", schema: "schema_name");
 
    //Specify default schema name applicable for all tables in the DbContext
    modelBuilder.HasDefaultSchema("schema_name");
  }
}
```
```c#
public class CustomDbContext : DbContext
{
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<ModelClass>( ).Property(temp => temp.PropertyName)
      .HasColumnName("column_name") //Specifies column name in table
      .HasColumnType("data_type") //Specifies column data type in table
      .HasDefaultValue("default_value") //Specifies default value of the column
  }
}
```
```c#
public class CustomDbContext : DbContext
{
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    //Adds database index for the specified column for faster searches
    modelBuilder.Entity<ModelClass>( ).HasIndex("column_name").IsUnique();
 
    //Adds check constraint for the specified column - that executes for insert & update
    modelBuilder.Entity<ModelClass>( ).HasCheckConstraint("constraint_name", "condition");
 }
}
```

## EF - Table Relations with Fluent API
### Table Relations

![table_relations](assets/table_relations.png)



### EF - Table Relations with Navigation Properties

![table_relations_w_navigation](assets/table_relations_w_navigation.png)



### EF - Table Relations with Fluent API
```c#
DbContext class

public class CustomDbContext : DbContext
{
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    //Specifies relation between primary key and foreign key among two tables
    modelBuilder.Entity<ChildModel>( )
     .HasOne<ParentModel>(parent => parent.ParentReferencePropertyInChildModel)
     .WithMany(child => child.ChildReferencePropertyInParentModel) //optional
     .HasForeignKey(child => child.ForeignKeyPropertyInChildModel)
  }
}
```



## EF - Async Operations
### async

- The method is awaitable.

- Can execute I/O bound code or CPU-bound code

### await

- Waits for the I/O bound or CPU-bound code execution gets completed.

- After completion, it returns the return value.





## Generate PDF Files

![generatepdf](assets/generatepdf.png)


### Rotativa.AspNetCore:
```c#
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
 
return new ViewAsPdf("ViewName", ModelObject, ViewData)
{
  PageMargins = new Margins() { Top = 1, Right = 2, Bottom = 3, Left = 4 },
  PageOrientation = Orientation.Landscape
}
```





## Generate CSV Files (CSVHelper)

![generate_csv](assets/generate_csv.png)


## CsvWriter:
### WriteRecords(records)

Writes all objects in the given collection.

Eg:
```csv
1,abc
2,def
```

### WriteHeader<ModelClass>( )

Writes all property names as headings.

Eg:

`Id, Name`



### WriteRecord(record)

Writes the given object as a row.

Eg:

`1, abc`



### WriteField( value )

Writes given value.



### NextRecord( )

Moves to the next line.



### Flush( )

Writes the current data to the stream.





## Generate Excel Files (EPPlus)

![epplus](assets/epplus.png)


## ExcelWorksheet
`["cell_address"].Value`

Sets or gets value at the specified cell.



`["cell_address"].Style`

Sets or gets formatting style of the specific cell.

# Interview Questions 

## What is Entity Framework?
Working with databases can often be rather complicated. You have to manage database connections, convert data from your application to a format the database can understand, and handle many other subtle issues.



The .NET ecosystem has libraries you can use for this, such as ADO.NET. However, it can still be complicated to manually build SQL queries and convert the data from the database into C# classes back and forth.



EF, which stands for Entity Framework, is a library that provides an object-oriented way to access a database. It acts as an object-relational mapper, communicates with the database, and maps database responses to .NET classes and objects.



Entity Framework (EF) Core is a lightweight, open-source, and cross-platform version of the Entity Framework.



Here are the essential differences between the two:

- **Cross-platform:** We can use EF Core in cross-platform apps that target .NET Core. EF 6.x targets .NET Framework, so you’re limited to Windows.

- **Performance:** EF Core is fast and lightweight. It significantly outperforms EF 6.x.

## “What other libraries or frameworks might you use with ASP.NET Core to build your application, and for what purposes?”
Since most applications need to store data, I’d likely reach for Entity Framework Core or Dapper for data access.

In addition, I’m also a big fan of FluentValidation to make validating user input easier to understand.

I’m pretty comfortable in writing unit tests, integration tests with xUnit, FluentAssertions and Moq. xUnit is quite advanced and extensible. FluentAssertions makes it easy to write human-readable & close-to-natural-language assert statements. Moq helps me to mock services.
## What is SQL injection attack?
A SQL injection attack is an attack mechanism used by hackers to steal sensitive information from the database of an organization. It is the application layer (means front-end) attack which takes benefit of inappropriate coding of our applications that allows a hacker to insert SQL commands into your code that is using SQL statement.



SQL Injection arises since the fields available for user input allow SQL statements to pass through and query the database directly. SQL Injection issue is a common issue with an ADO.NET Data Services query.
## How to handle SQL injection attacks in Entity Framework?
Entity Framework is injection safe since it always generates parameterized SQL commands which help to protect our database against SQL Injection.



A SQL injection attack can be made in Entity SQL syntax by providing some malicious inputs that are used in a query and in parameter names. To avoid this one, you should never combine user inputs with Entity SQL command text.

## What are POCO classes?
The term POCO does not mean to imply that your classes are either plain or old. The term POCO simply specifies that the POCO classes don’t contain any reference that is specific to the entity framework or .NET framework.



Basically, POCO (Plain Old CLR Object) entities are existing domain objects within your application that you use with Entity Framework.

Eg:

```c#
public class Model
{
  public type PropertyName1 { get; set; }
  public type PropertyName2 { get; set; }
}
```
## What is the proxy object?
An object that is created from a POCO class (model class) to support change tracking and lazy loading, is known as a proxy object.

There are some rules for creating a proxy class object:

- The class must be public and not sealed.

- Each navigation property must be marked as virtual.

- Each property must have a public getter and setter.

- Any collection navigation properties must be typed as ICollection <T>.
## What are the various Entity States in EF?
Each and every entity has a state during its lifecycle which is defined by an enum (EntityState) that have the following values:

- Added

- Modified

- Deleted

- Unchanged

- Detached
## What are various approaches in Code First for model designing?
In Entity Framework Code First approach, our POCO classes are mapped to the database objects using a set of conventions defined in Entity Framework. If you do not want to follow these conventions while defining your POCO classes, or you want to change the way the conventions work then you can use the fluent API or data annotations to configure and to map your POCO classes to the database tables. There are two approaches, which you can use to define the model in EF Code First:
- POCO model classes with data annotations

- POCO model classes with FluentAPI in DbContext

## What C# Datatype is mapped with which Datatype in SQL Server?
| C# Data Type   | SQL server data type|
|:-------------   | :------------------|
| int	           |  int              |
| string	       | nvarchar(Max)     |
| decimal	       | decimal(18,2)     |
| float	       | real                  |
| byte[]	       | varbinary(Max)    |
| datetime       | 	datetime           |
| bool	       | bit                   |
| byte	       | tinyint               |
| short	       | smallint              |
| long	       | bigint                |
| double	       | float             |
| char	       | No mapping            |
| sbyte	       | No mapping            |
| object	       | No mapping        |
## What is Code First Migrations in Entity Framework?
Code First Migrations allow you to create a new database or to update the existing database based on your model classes by using Package Manager Console exist within Visual Studio.


## What is Migrations History Table?
In EF Core, Migrations history table (`__MigrationHistory`) is a part of the application database and used by Code First Migrations to store details about migrations applied to a database. This table is created when you apply the first migration to the database. This table stores metadata describing the schema version history of one or more EF Code First models within a given database.
## How you apply code first migrations through code in EF Core?
Entity Framework Core supports “Migrate()” method, which can be called anywhere either in controller or in any other middleware when you want to perform code first migrations programmatically instead of applying migrations using “Update-Database” command.

Eg:
```c#
using (var context = new MyDbContext(...))
{
    context.Database.Migrate();
}
```