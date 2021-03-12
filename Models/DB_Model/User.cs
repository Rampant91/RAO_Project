using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DB_Model
{
    class User
    {
        private UserInfo _userInfo = new UserInfo();
        public UserInfo UserInfo
        {
            get
            {
                return _userInfo;
            }
            set
            {
                _userInfo = value;
            }
        }
        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        private string _login = "";
        public string Login
        {
            get
            {
                return _login;
            }
            set
            {
                _login = value;
            }
        }
        private string _password = "";
        public string Password
        {
            get 
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }
        private Token _token = new Token();
        public Token Token
        {
            get
            {
                return _token;
            }
            set
            {
                _token = value;
            }
        }
        private int _rights;
        public int Rights
        {
            get
            {
                return _rights;
            }
            set
            {
                _rights = value;
            }
        }
        public User() { }
        public User(int id, string login, string password, Token token, int rights, UserInfo userInfo)
        {
            UserInfo = userInfo;
            Id = id;
            Login = login;
            Password = password;
            Token = token;
            Rights = rights;
        }
    }
}
