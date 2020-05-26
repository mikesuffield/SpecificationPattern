# Specification Pattern with Dapper

This is an example project to demonstrate how the specification pattern can be implemented into a service that makes use of [Dapper](https://github.com/StackExchange/Dapper).

## Introduction

In English, a specification is defined as 
> an act of identifying something precisely or of **stating a precise requirement**

...and in Domain Driven Design, the premise is the same.

Specifications encourage the use of the Single Responsibility Principle to segregate and encapsulate business rules into individual “specifications”, which can easily be re-used to avoid duplication of similar logic. They encapsulate *one* piece of logic only – nothing more. 

The two most common use cases for using specifications are:
- In memory validation of an object
- Data retrieval

Specifications can be chained together using basic Boolean operands (AND, OR, NOT etc) to form more complex domain logic, which can make complex database queries easier to read and put together. This means that they can be thought of as the "building blocks", with which you can build up rules for filter domain model objects.

### Pros

- Encapsulate domain knowledge into a single unit
- Avoid duplication of domain knowledge (DRY) by providing a centralised location for this logic
- Declarative by design, which increases maintainability

### Cons

- Implementation can tie you to a specific database implementation (e.g. SQL)

## Example Project Overview

The example project is a service that is used to list menu items for a chinese restaurant. The aggregate root in this case is the MenuItem, and each MenuItem has a list of potential allergens. The user is able to filter the list of MenuItems based on the Type of meal (starter, main, dessert), as well as the presence (or lack) of specific allergens.

## Resources

* *Specifications* by Eric Evans & Martin Fowler - https://bit.ly/spec-pattern

## License

This project is licensed under the terms of the [MIT license](https://github.com/mikesuffield/SpecificationPattern/blob/master/LICENSE)