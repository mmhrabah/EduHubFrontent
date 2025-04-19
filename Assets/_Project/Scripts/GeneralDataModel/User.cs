using System;

[Serializable]
public class User
{
    public Guid Id;
    public string Username;
    public string Password;
    public string Email;
    public string PhoneNumber;
    public DateTime DateOfBirth;
    public string Address;
    public string ProfilePictureUrl;
    public DateTime CreatedAt;
    public DateTime UpdatedAt;
}