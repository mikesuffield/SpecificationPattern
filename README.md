# Specification Pattern with Dapper

This is an example project to demonstrate how the specification pattern can be implemented into a service that makes use of [Dapper](https://github.com/StackExchange/Dapper).

## Introduction

In English, a specification is defined as 
> an act of identifying something precisely or of **stating a precise requirement**

...and in Domain Driven Design, the premise is the same.

Specifications encourage the use of the Single Responsibility Principle to segregate and encapsulate business rules into individual “specifications”, which can easily be re-used to avoid duplicating similar logic. They encapsulate *one* piece of logic only – nothing more. 

The two most common use cases for using specifications are:
- In-memory validation of an object
- Data retrieval

Specifications can be chained together using basic Boolean operands (AND, OR, NOT etc) to form more complex domain logic, which can make complex database queries easier to read and put together. This means that they can be thought of as the "building blocks", with which you can build up rules for filtering domain model objects.

### Pros

- Encapsulate domain knowledge into a single unit
- Avoid duplication of domain knowledge (DRY) by providing a centralised location for this logic
- Declarative by design, which increases maintainability

### Cons

- Implementation can tie you to a specific database implementation (e.g. SQL)
- Some may argue that it is only worth implementing this pattern if it solves more than one use case for your project (i.e. you will be using specifications for data retrieval *and* in-memory validation)
- Separation of logic into multiple specifications could fragment an otherwise cohesive project if the separation isn't required

## Example Project Overview

The example project is a service that is used to list menu items for a Chinese restaurant - this could be consumed by an on-premise POS system, by a fast food delivery mobile app, or by the restaurant website to simply display the menu. 

The aggregate root in this case is the `MenuItem`, and each MenuItem has a list of `Allergens`. The user is able to filter the list of MenuItems using two parameters - the `MealType` (starter, main, dessert) and/or excluding meals that contain specific allergens.

The business logic for these two filters can be encapsulated in two specifications - `MenuItemForMealTypeSpecification` and `MenuItemForAllergensSpecification`.

### Code snippet

Below is the code snippet for the `MenuItemForMealTypeSpecification`. By design, all specifications implement the `IsSatisfiedBy` method which returns a bool, which we can use to validate objects in memory. In addition to this, a second `IsSatisfiedBy` method has been introduced to build up the SQL required by Dapper to implement this business rule - this means we can use the same specification when querying the database via Dapper to filter data. Refer to the project for full implementation.

```csharp
public sealed class MenuItemForMealTypeSpecification : ISpecification<MenuItem>
{
    private readonly MealType _mealType;

    public MenuItemForMealTypeSpecification(MealType mealType)
    {
        _mealType = mealType;
    }

    public bool IsSatisfiedBy(MenuItem entity)
    {
        return entity.MealType == _mealType;
    }

    public void IsSatisfiedBy(StringBuilder builder, IDictionary<string, object> parameters)
    {
        builder.Append("[MealType] = @mealType");
        parameters.Add("mealType", _mealType.ToString());
    }
}
```
### Notes

- It is recommended that specifications are as precise as possible, which means avoiding parameterisation if you can. In the example above, we could have created three separate specifications for each meal type (starter, main and dessert) to avoid passing through the "mealType" parameter in the constructor - this would make the specification less generic and more specific. However, in this case I have taken the decision use the parameterized specification vs multiple precise specification as I believe the passing through the parameter makes more sense given our use case.
- Database indexes should be kept in mind when creating specifications in this way - in the example above, it is important to ensure there is suitable index on the `MealType` column in the database to avoid performance issues.

## Resources

- [Specifications](https://bit.ly/spec-pattern) - Eric Evans & Martin Fowler
- [DapperUnitOfWork](https://github.com/timschreiber/DapperUnitOfWork ) - Tim Schreiber
- [Generic repository pattern using Dapper](https://itnext.io/generic-repository-pattern-using-dapper-bd48d9cd7ead) - Damir Bolic
- [Repository pattern and unit of work using dapper](https://alexcodetuts.com/2020/03/07/asp-net-core-3-1-repository-pattern-and-unit-of-work-using-dapper/) - Alex Calingasan

## License

This project is licensed under the terms of the [MIT license](https://github.com/mikesuffield/SpecificationPattern/blob/master/LICENSE)
