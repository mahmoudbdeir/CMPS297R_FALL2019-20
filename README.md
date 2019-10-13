# CMPS 297R FALL 2019-2020
This is a sample program on how to code with Microsoft Azure Queues. 

## To run the program:
You must have Visual Studio 2019 Community Edition and .NET Core 3.0 installed.

1. Create a Storage Account on Microsoft Azure
2. Copy the connection string from the Azure portal located under 'Keys'
3. Add an environment variable called ```CloudStorageConnectionString``` whose value is the connection string you got from step 2
4. Compile the program and run it twice from two different command prompts, one with an command line argument of ```/r``` and the other with ```/w```
5. You can also test sending a receiving JSON objects by running with ```/ro``` and ```/wo``` arguments.
