```sh
dotnet new sln -o BuberBreakfast
cd BuberBreakfast
dotnet new classlib -o BuberBreakfast.Contracts
dotnet new webapi -o BuberBreakfast
dotnet build

dotnet sln add BuberBreakfast.Contracts BuberBreakfast
# OR
dotnet sln add (ls -r **/*.csproj)

dotnet add ./BuberBreakfast/ reference ./BuberBreakfast.Contracts
```