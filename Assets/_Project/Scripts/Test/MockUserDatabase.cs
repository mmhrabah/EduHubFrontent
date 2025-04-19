using System;
using System.Collections.Generic;
using System.Net;
using Rabah.Screens;
using Rabah.Utils.Network;
using UnityEngine;

[CreateAssetMenu(fileName = "MockUserDatabase", menuName = "ScriptableObjects/MockUserDatabase", order = 1)]
public class MockUserDatabase : ScriptableObject
{
    public List<User> Users = new();
    public Dictionary<Guid, User> UserDictionary = new();

    public void FillUserDictionary()
    {
        for (int i = 0; i < Users.Count; i++)
        {
            Guid guid = Guid.Parse(Users[i].Id);
            if (!UserDictionary.ContainsKey(guid))
            {
                UserDictionary.Add(guid, Users[i]);
            }
        }
    }

    public ResponseModel<User> AddUser(User user)
    {
        var existingUser = Users.Find(u => u.Username == user.Username);
        if (existingUser == null)
        // Add the new user to the list and dictionary
        {
            user.Id = Guid.NewGuid().ToString();
            Users.Add(user);
            Guid guid = Guid.Parse(user.Id);
            UserDictionary.Add(guid, user);
            return new ResponseModel<User>
            {
                StatusCode = (int)HttpStatusCode.Created,
                Data = user
            };
        }
        return new ResponseModel<User>
        {
            StatusCode = (int)HttpStatusCode.BadRequest,
            Data = null
        };
    }


    public ResponseModel<User> GetUser(Guid id)
    {
        if (UserDictionary.TryGetValue(id, out User user))
        {
            return new ResponseModel<User>
            {
                StatusCode = (int)HttpStatusCode.OK,
                Data = user
            };
        }
        return new ResponseModel<User>
        {
            StatusCode = (int)HttpStatusCode.NotFound,
            Data = null
        };
    }

    public ResponseModel<User> Login(string username, string password)
    {
        var existingUser = Users.Find(u => u.Username == username && u.Password == password);

        if (existingUser == null)
        {
            return new ResponseModel<User>
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
                Data = null
            };
        }

        return new ResponseModel<User>
        {
            StatusCode = (int)HttpStatusCode.OK,
            Data = existingUser
        };
    }
}
