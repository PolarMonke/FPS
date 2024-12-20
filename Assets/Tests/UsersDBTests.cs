//using NUnit.Framework;
using UnityEngine;

public class UsersDBTests
{
    //[Test]
    public void TestMyMethod()
    {
        UsersDB usersDB = new UsersDB();
        string username = "TestUser";
        string password = "TestPassword123";
        bool  expectedOutput = true;

        usersDB.AddUser(username, password);
        bool actualOutput = usersDB.SearchUser(username);
        
        //Assert.AreEqual(expectedOutput, actualOutput);
    }

}
