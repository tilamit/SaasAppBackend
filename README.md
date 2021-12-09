# SaasApp

This project was built with .NET Core 5.

## Development Server

Run `dotnet run` for a dev server. Navigate to `http://localhost:5020`. To make any changes to a file, press ctrl + c and after making changes, run `dotnet run` again.

## Database

Data is currently hosted in Microsoft Azure and it has elastic pool enabled.

## Api Testing

The app uses Rest Client plugin to test api. A file `rest-api.http` is provided to do the api testing, click on `Send Request` to see the response.

##Further instruction

If any issue arises while running the app, check the `TodoApi.csproj` file. It has the .NET core version and details of installed packages.

1) A grid is shown in a web page and it shows details like who are paid users or vice-versa. A form is given for initial entry of a user

2) A check box indicates if checked, then user is a paid user or vice-versa. If user is unchecked and requires to be a paid user, then tick on the check box in the grid and a
   database should be generated in Azure with a row in `Users` table (Will take a bit). On the other hand, if user is checked which indicates paid user and requires to be in free trial mode, then uncheck the check box. 

3) To verify a new user created, refresh the web page 
   
4) To check created database in Sql Server, user this - use DbName (Auto generated database name) select * from Users; --(May not work in some cases)