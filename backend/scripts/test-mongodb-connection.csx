#!/usr/bin/env dotnet-script
#r "nuget: MongoDB.Driver, 3.5.0"

using MongoDB.Driver;
using System;

Console.WriteLine("Testing MongoDB Connection...");
Console.WriteLine("==============================");

try
{
    // Read connection string from environment or use default
    var connectionString = Environment.GetEnvironmentVariable("MongoDbSettings__ConnectionString")
        ?? "mongodb://localhost:27017";
    var databaseName = Environment.GetEnvironmentVariable("MongoDbSettings__DatabaseName")
        ?? "RealEstateDB";

    Console.WriteLine($"Connection String: {connectionString}");
    Console.WriteLine($"Database Name: {databaseName}");
    Console.WriteLine();

    // Create MongoDB client
    var client = new MongoClient(connectionString);

    // Test connection by listing databases
    Console.WriteLine("Attempting to connect to MongoDB...");
    var databases = await client.ListDatabaseNamesAsync();
    var dbList = await databases.ToListAsync();

    Console.WriteLine("✅ Successfully connected to MongoDB!");
    Console.WriteLine();
    Console.WriteLine("Available databases:");
    foreach (var db in dbList)
    {
        Console.WriteLine($"  - {db}");
    }

    // Check if RealEstateDB exists
    var database = client.GetDatabase(databaseName);
    var collections = await database.ListCollectionNamesAsync();
    var collectionList = await collections.ToListAsync();

    Console.WriteLine();
    Console.WriteLine($"Collections in '{databaseName}':");
    if (collectionList.Count == 0)
    {
        Console.WriteLine("  ⚠️  No collections found. Run 'mongosh < scripts/init-mongodb.js' to initialize the database.");
    }
    else
    {
        foreach (var collection in collectionList)
        {
            Console.WriteLine($"  ✅ {collection}");
        }
    }

    Console.WriteLine();
    Console.WriteLine("✅ MongoDB connection test completed successfully!");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error connecting to MongoDB: {ex.Message}");
    Console.WriteLine();
    Console.WriteLine("Make sure MongoDB is running:");
    Console.WriteLine("  - Install MongoDB: https://www.mongodb.com/try/download/community");
    Console.WriteLine("  - Start MongoDB: brew services start mongodb-community (macOS)");
    Console.WriteLine("  - Or use MongoDB Atlas: https://www.mongodb.com/cloud/atlas");
    Environment.Exit(1);
}
