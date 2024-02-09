# E-Commerce Orders App
## Requirement:

Imagine an e-Commerce application. Create an Asp.Net Core Web Application that receives orders from the customers.



An order contains order date, list of products (each product has product code number, price per one unit and quantity) and invoice price (the total cost of all products  in the order).

If no validation errors, the application should generate a new order number (random number between 1 and 99999) and sent it as response.



Consider a model class called 'Order' with following properties:

- int? OrderNo

- DateTime OrderDate

- double InvoicePrice

- List<Product> Products



Consider a model class called 'Product' with following properties:

- int ProductCode

- double Price

- int Quantity