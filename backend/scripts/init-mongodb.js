// MongoDB Initialization Script
// Run this script with: mongosh < scripts/init-mongodb.js

// Switch to the RealEstateDB database
db = db.getSiblingDB('RealEstateDB');

// Create collections
db.createCollection('Owners');
db.createCollection('Properties');
db.createCollection('PropertyImages');
db.createCollection('PropertyTraces');

// Create indexes for Owners collection
db.Owners.createIndex({ "name": 1 });

// Create indexes for Properties collection
db.Properties.createIndex({ "name": 1 });
db.Properties.createIndex({ "address": 1 });
db.Properties.createIndex({ "price": 1 });
db.Properties.createIndex({ "year": 1 });
db.Properties.createIndex({ "idOwner": 1 });
db.Properties.createIndex({ "codeInternal": 1 });

// Create indexes for PropertyImages collection
db.PropertyImages.createIndex({ "idProperty": 1 });
db.PropertyImages.createIndex({ "enabled": 1 });
db.PropertyImages.createIndex({ "idProperty": 1, "enabled": 1 });

// Create indexes for PropertyTraces collection
db.PropertyTraces.createIndex({ "idProperty": 1 });
db.PropertyTraces.createIndex({ "dateSale": -1 });

print("MongoDB database 'RealEstateDB' initialized successfully!");
print("Collections created: Owners, Properties, PropertyImages, PropertyTraces");
print("Indexes created for all collections");
