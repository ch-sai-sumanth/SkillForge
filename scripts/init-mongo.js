// MongoDB initialization script
// This script runs when the MongoDB container starts for the first time

// Switch to the SkillForge database
db = db.getSiblingDB('SkillForgeDB');

// Create a regular user for the application
db.createUser({
  user: 'skillforge_user',
  pwd: 'skillforge_password',
  roles: [
      
    {
      role: 'readWrite',
      db: 'SkillForgeDB'
    }
  ]
});

// Create collections with basic indexes for better performance
db.createCollection('users');
db.users.createIndex({ "email": 1 }, { unique: true });
db.users.createIndex({ "username": 1 }, { unique: true });

db.createCollection('sessions');
db.sessions.createIndex({ "mentorId": 1 });
db.sessions.createIndex({ "menteeId": 1 });
db.sessions.createIndex({ "scheduledAt": 1 });

db.createCollection('learningGoals');
db.learningGoals.createIndex({ "menteeId": 1 });

db.createCollection('learningTasks');
db.learningTasks.createIndex({ "menteeId": 1 });
db.learningTasks.createIndex({ "goalId": 1 });

db.createCollection('payments');
db.payments.createIndex({ "userId": 1 });
db.payments.createIndex({ "status": 1 });

db.createCollection('subscriptionPlans');
db.subscriptionPlans.createIndex({ "name": 1 }, { unique: true });

db.createCollection('userSubscriptions');
db.userSubscriptions.createIndex({ "userId": 1 });
db.userSubscriptions.createIndex({ "subscriptionPlanId": 1 });

db.createCollection('mentorRates');
db.mentorRates.createIndex({ "mentorId": 1 }, { unique: true });

db.createCollection('sessionHistories');
db.sessionHistories.createIndex({ "sessionId": 1 });
db.sessionHistories.createIndex({ "mentorId": 1 });
db.sessionHistories.createIndex({ "menteeId": 1 });

print('Database initialized successfully with collections and indexes');

// Insert sample data (optional - for development/demo purposes)
if (db.users.countDocuments() === 0) {
  print('Inserting sample data...');
  
  // Sample subscription plans
  db.subscriptionPlans.insertMany([
    {
      _id: ObjectId(),
      name: "Basic",
      description: "Basic mentorship plan with limited features",
      price: 29.99,
      durationInDays: 30,
      features: ["Basic mentor matching", "Monthly goals", "Email support"],
      isActive: true,
      createdAt: new Date(),
      updatedAt: new Date()
    },
    {
      _id: ObjectId(),
      name: "Premium",
      description: "Premium mentorship plan with advanced features",
      price: 79.99,
      durationInDays: 30,
      features: ["Advanced mentor matching", "Unlimited goals", "Video sessions", "Priority support"],
      isActive: true,
      createdAt: new Date(),
      updatedAt: new Date()
    },
    {
      _id: ObjectId(),
      name: "Enterprise",
      description: "Enterprise plan for organizations",
      price: 199.99,
      durationInDays: 30,
      features: ["All premium features", "Team management", "Analytics", "Dedicated support"],
      isActive: true,
      createdAt: new Date(),
      updatedAt: new Date()
    }
  ]);
  
  print('Sample subscription plans inserted');
}

print('MongoDB initialization completed!');