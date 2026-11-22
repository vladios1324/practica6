using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace RegIN_Matckevich_Alikin.Classes
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public byte[] Image = new byte[0];
        public DateTime DateUpdate { get; set; }
        public DateTime DateCreate { get; set; }
        public CorrectLogin HandlerCorrectLogin;
        public InCorrectLogin HandlerInCorrectLogin;

        public delegate void CorrectLogin();
        public delegate void InCorrectLogin();

        public void GetUserLogin(string login)
        {
            Id = -1;
            Login = String.Empty;
            Password = String.Empty;
            Name = String.Empty;
            Image = new byte[0];

            MySqlConnection mySqlConnection = WorkingDB.OpenConnection();

            if (WorkingDB.IsOpen(mySqlConnection))
            {
                using (MySqlDataReader userQuery = WorkingDB.Query($"SELECT * FROM `users` WHERE `Login` = '{login}'", mySqlConnection))
                {
                    if (userQuery.HasRows)
                    {
                        userQuery.Read();
                        Id = userQuery.GetInt32(0);
                        Login = userQuery.GetString(1);
                        Password = userQuery.GetString(2);
                        Name = userQuery.GetString(3);
                        if (!userQuery.IsDBNull(4))
                        {
                            Image = new byte[64 * 1024];
                            userQuery.GetBytes(4, 0, Image, 0, Image.Length);
                        }

                        DateUpdate = userQuery.GetDateTime(5);
                        DateCreate = userQuery.GetDateTime(6);
                        HandlerCorrectLogin.Invoke();
                    }
                    else
                        HandlerInCorrectLogin.Invoke();
                }
            }
            else
                HandlerInCorrectLogin.Invoke();

            WorkingDB.CloseConnection(mySqlConnection);
        }

        public void SetUser()
        {
            MySqlConnection mySqlConnection = WorkingDB.OpenConnection();

            if(WorkingDB.IsOpen(mySqlConnection))
            {
                MySqlCommand mySqlCommand = new MySqlCommand("INSERT INTO `users`(`Login`, `Password`, `Name`, `Image`, `DateUpdate`, `DateCreate`) VALUES (@Login, @Password, @Name, @Image, @DateUpdate, @DateCreate)", mySqlConnection);

                mySqlCommand.Parameters.AddWithValue("@Login", Login);
                mySqlCommand.Parameters.AddWithValue("@Password", Password);
                mySqlCommand.Parameters.AddWithValue("@Name", Name);
                mySqlCommand.Parameters.AddWithValue("@Image", Image);
                mySqlCommand.Parameters.AddWithValue("@DateUpdate", DateUpdate);
                mySqlCommand.Parameters.AddWithValue("@DateCreate", DateCreate);

                mySqlCommand.ExecuteNonQuery();
            }

            WorkingDB.CloseConnection(mySqlConnection);
        }

        public void CreateNewPassword()
        {
            if(Login != String.Empty)
            {
                Password = GeneratePass();

                MySqlConnection mySqlConnection = WorkingDB.OpenConnection();
                if(WorkingDB.IsOpen(mySqlConnection))
                {
                    WorkingDB.Query($"UPDATE `users` SET `Password`='{Password}' WHERE `Login` = '{Login}'", mySqlConnection);
                }

                WorkingDB.CloseConnection(mySqlConnection);
                SendMail.SendMessage($"Your account password has been changed.\nNew password: {Password}", Login);
            }
        }

        public string GeneratePass()
        {
            List<char> NewPassword = new List<char>();
            Random rnd = new Random();
            char[] ArrNumbers = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            char[] ArrSymbols = { '|', '-', '_', '!', '@', '#', '$', '%', '&', '*', '=', '+' };
            char[] ArrLetters = "qwertyuiopasdfghjklzxcvbnm".ToCharArray();

            NewPassword.Add(ArrNumbers[rnd.Next(ArrNumbers.Length)]);
            NewPassword.Add(ArrSymbols[rnd.Next(ArrSymbols.Length)]);

            for (int i = 0; i < 2; i++)
                NewPassword.Add(char.ToUpper(ArrLetters[rnd.Next(ArrLetters.Length)]));

            for (int i = 0; i < 6; i++)
                NewPassword.Add(ArrLetters[rnd.Next(ArrLetters.Length)]);

            for (int i = 0; i < NewPassword.Count; i++)
            {
                int RandomSymbol = rnd.Next(NewPassword.Count);
                char Symbol = NewPassword[RandomSymbol];
                NewPassword[RandomSymbol] = NewPassword[i];
                NewPassword[i] = Symbol;
            }

            return new string(NewPassword.ToArray());
        }
    }
}
