using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dbread : MonoBehaviour
{
    public Text status;
    public InputField inputUser, inputPass, regUsername, regPassword, regEmail;
    int currentID;
    bool takenUsername;

    string giantString;

    public string[] registeredUsers;
    public string[] usernames = new string[100];
    public string[] passwords = new string[100];

     IEnumerator Start()
    {
        WWW users = new WWW("http://localhost/read.php");
        yield return users;

        giantString = users.text;

        registeredUsers = giantString.Split(';');

        for(int i=0; i<registeredUsers.Length-1; i++)
        {
            usernames[i] = registeredUsers[i].Substring(registeredUsers[i].IndexOf('U') + 9);
            usernames[i] = usernames[i].Remove(usernames[i].IndexOf('|'));

            passwords[i] = registeredUsers[i].Substring(registeredUsers[i].IndexOf("Password") + 9);
        }
    }

    public void tryToLogin()
    {
        currentID = -1;

        if(inputUser.text == "" || inputPass.text == "")
        {
            status.text = "UserName or Password can't be empty!";
        }
        else
        {
            for(int i=0; i<registeredUsers.Length-1; i++)
            {
                if(inputUser.text == usernames[i])
                {
                    currentID = i;
                }
            }
            if(currentID == -1)
            {
                status.text = "User not Found!";
            }
            else
            {
                if(inputPass.text == passwords[currentID])
                {
                    status.text = "Success!";
                }
                else
                {
                    status.text = "Wrong Password!";
                }
            }
        }
    }

    public void tryToRegister()
    {
        takenUsername = false;
        
        if(regUsername.text == "" || regPassword.text == "" || regEmail.text == "")
        {
            status.text = "No Empty Fields Allowed!";
        }
        else
        {
            for(int i=0; i<registeredUsers.Length-1; i++)
            {
                if(regUsername.text == usernames[i])
                {
                    takenUsername = true;                   
                }
            }

            if(takenUsername == false && regUsername.text != "Password")
            {
                registerUser(regUsername.text, regPassword.text, regEmail.text);
                status.text = "Registration successfull!";
            }
            else
            {
                status.text = "Username already exist!";
            }
        }
    }

    public void registerUser(string username, string password, string email)
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("passwordPost", password);
        form.AddField("emailPost", email);

        WWW register = new WWW("http://localhost/insertuser.php", form);
    }
}
