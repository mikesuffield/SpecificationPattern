# Specification Pattern with Dapper (.NET Core 3.1)
This is an example project to demonstrate how the specification pattern can be implemented into a service that makes use of [Dapper](https://github.com/StackExchange/Dapper).

## Introduction
The specification pattern is ...

## Example Project Overview
The example project is a service that is used to list menu items for a chinese restaurant. The aggregate root in this case is the MenuItem, and each MenuItem has a list of potential allergens. The user is able to filter the list of MenuItems based on the Type of meal (starter, main, dessert), as well as the presence (or lack) of specific allergens.

## Resources
* *Specifications* by Eric Evans & Martin Fowler - https://bit.ly/spec-pattern

## License
This project is licensed under the terms of the [MIT license](https://github.com/mikesuffield/SpecificationPattern/blob/master/LICENSE)