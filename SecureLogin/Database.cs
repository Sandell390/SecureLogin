﻿using MySql.Data.MySqlClient;

namespace SecureLogin;

using MySql.Data;

public class Database
{
    private string connString = @"Server=127.0.0.1;Database=secure_password;Uid=root;Pwd=Muffin123;";
    
    public bool DoesUserExist(string username)
    {
        using (MySqlConnection conn = new MySqlConnection(connString))
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM users WHERE username = @username", conn);
            cmd.Parameters.AddWithValue("@username", username);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    
    public void Register(string username, string passhash)
    {
        using (MySqlConnection conn = new MySqlConnection(connString))
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO users (username, passhash, login_status, login_attempts) VALUES (@username, @passhash, @login_status, @login_attempts)", conn);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@passhash", passhash);
            cmd.Parameters.AddWithValue("@login_status", 1);
            cmd.Parameters.AddWithValue("@login_attempts", 0);
            cmd.ExecuteNonQuery();
        }
    }
    
    public int GetLoginAttempts(string username)
    {
        using (MySqlConnection conn = new MySqlConnection(connString))
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT login_attempts FROM users WHERE username = @username", conn);
            cmd.Parameters.AddWithValue("@username", username);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
    }
    
    public void UpdateLoginAttempts(string username, int attempts)
    {
        using (MySqlConnection conn = new MySqlConnection(connString))
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("UPDATE users SET login_attempts = @attempts WHERE username = @username", conn);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@attempts", attempts);
            cmd.ExecuteNonQuery();
        }
    }
    
    public void ResetLoginAttempts(string username)
    {
        using (MySqlConnection conn = new MySqlConnection(connString))
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("UPDATE users SET login_attempts = 0 WHERE username = @username", conn);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.ExecuteNonQuery();
        }
    }
    
    public void UpdateLoginStatus(string username, int status)
    {
        using (MySqlConnection conn = new MySqlConnection(connString))
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("UPDATE users SET login_status = @status WHERE username = @username", conn);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.ExecuteNonQuery();
        }
    }

    public void UpdateSalt(string username, string salt)
    {
        using (MySqlConnection conn = new MySqlConnection(connString))
        {
            conn.Open();
            // Get username id from database
            MySqlCommand cmd = new MySqlCommand("SELECT iduser FROM users WHERE username = @username", conn);
            cmd.Parameters.AddWithValue("@username", username);
            int id = Convert.ToInt32(cmd.ExecuteScalar());
            // Update salt
            cmd = new MySqlCommand("UPDATE salts SET salt = @salt WHERE iduser = @id", conn);
            cmd.Parameters.AddWithValue("@salt", salt);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }

    public void RegisterSalt(string username, string salt)
    {
        using (MySqlConnection conn = new MySqlConnection(connString))
        {
            conn.Open();
            // Get username id from database
            MySqlCommand cmd = new MySqlCommand("SELECT iduser FROM users WHERE username = @username", conn);
            cmd.Parameters.AddWithValue("@username", username);
            int id = Convert.ToInt32(cmd.ExecuteScalar());
            // Update salt
            cmd = new MySqlCommand("INSERT INTO salts (salt, userid) VALUES (@salt, @userid)", conn);
            cmd.Parameters.AddWithValue("@salt", salt);
            cmd.Parameters.AddWithValue("@userid", id);
            cmd.ExecuteNonQuery();
        }
    }
    
    public string GetSalt(string username)
    {
        using (MySqlConnection conn = new MySqlConnection(connString))
        {
            conn.Open();
            // Get username id from database
            MySqlCommand cmd = new MySqlCommand("SELECT iduser FROM users WHERE username = @username", conn);
            cmd.Parameters.AddWithValue("@username", username);
            int id = Convert.ToInt32(cmd.ExecuteScalar());
            // Get salt
            cmd = new MySqlCommand("SELECT salt FROM salts WHERE iduser = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            return cmd.ExecuteScalar().ToString();
        }
    }
    
    public string GetPassHash(string username)
    {
        using (MySqlConnection conn = new MySqlConnection(connString))
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT passhash FROM users WHERE username = @username", conn);
            cmd.Parameters.AddWithValue("@username", username);
            return cmd.ExecuteScalar().ToString();
        }
    }
    
    public void UpdatePassHash(string username, string passhash)
    {
        using (MySqlConnection conn = new MySqlConnection(connString))
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("UPDATE users SET passhash = @passhash WHERE username = @username", conn);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@passhash", passhash);
            cmd.ExecuteNonQuery();
        }
    }
    
}